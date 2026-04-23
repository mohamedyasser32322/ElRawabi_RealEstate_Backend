using AutoMapper;
using ElRawabi_RealEstate_Backend.Dtos.Responses;
using ElRawabi_RealEstate_Backend.DTOs.Requests;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Helpers;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class ConstructionStageService : IConstructionStageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConstructionStageService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IActivityLogService activityLogService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _activityLogService = activityLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Builds the base URL once from the current HTTP request context
        private string GetBaseUrl()
        {
            var req = _httpContextAccessor.HttpContext?.Request;
            if (req == null) return string.Empty;
            return $"{req.Scheme}://{req.Host}";
        }

        public async Task<IEnumerable<ConstructionStageResponseDto>> GetAllConstructionStagesAsync()
        {
            var stages = await _unitOfWork.ConstructionStages.GetAllConstructionStagesAsync();
            var baseUrl = GetBaseUrl();
            return stages.Select(s => MapToResponse(s, baseUrl));
        }

        public async Task<IEnumerable<ConstructionStageResponseDto>> GetByBuildingIdAsync(int buildingId)
        {
            var stages = await _unitOfWork.ConstructionStages.GetConstructionStagesByBuildingIdAsync(buildingId);
            var baseUrl = GetBaseUrl();
            return stages.Select(s => MapToResponse(s, baseUrl));
        }

        public async Task<ConstructionStageResponseDto?> GetConstructionStageByIdAsync(int id)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            return stage == null ? null : MapToResponse(stage, GetBaseUrl());
        }

        public async Task<ConstructionStageResponseDto> CreateConstructionStageAsync(
            ConstructionStageRequestDto dto,
            int? currentUserId)
        {
            var stage = new ConstructionStage
            {
                BuildingId = dto.BuildingId,
                StageName = dto.StageName,
                Status = SyncStatus(dto.Status, dto.IsCompleted),
                IsCompleted = dto.IsCompleted,
                Notes = dto.Notes,
                ReportData = dto.ReportData,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            if (stage.IsCompleted && stage.StartDate == null)
                stage.StartDate = DateTime.UtcNow;

            await _unitOfWork.ConstructionStages.AddConstructionStageAsync(stage);
            await _unitOfWork.CompleteAsync();

            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(stage.BuildingId);
            var project = building != null
                ? await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId)
                : null;

            var stageAr = StageNameHelper.ToArabic(stage.StageName);

            var newSnapshot = new
            {
                StageName = stageAr,
                Status = stage.Status.ToString(),
                IsCompleted = stage.IsCompleted,
                Notes = stage.Notes,
                EndDate = stage.EndDate,
                ReportData = stage.ReportData,
                BuildingName = building?.Name,
                ProjectName = project?.Name
            };

            await _activityLogService.LogActivityAsync(
                "إضافة", "مرحلة بناء", stage.Id,
                $"إضافة مرحلة '{stageAr}' — مبنى: {building?.Name ?? stage.BuildingId.ToString()} — مشروع: {project?.Name ?? "—"}",
                currentUserId,
                newValues: newSnapshot);

            // Re-fetch so navigation properties (Images, Building) are populated
            var created = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(stage.Id);
            return MapToResponse(created ?? stage, GetBaseUrl());
        }

        public async Task<bool> UpdateConstructionStageAsync(
            int id,
            ConstructionStageRequestDto dto,
            int? currentUserId)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return false;

            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(stage.BuildingId);
            var project = building != null
                ? await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId)
                : null;

            var stageAr = StageNameHelper.ToArabic(stage.StageName);

            var oldSnapshot = new
            {
                StageName = stageAr,
                Status = stage.Status.ToString(),
                IsCompleted = stage.IsCompleted,
                Notes = stage.Notes,
                EndDate = stage.EndDate,
                ReportData = stage.ReportData,
                BuildingName = building?.Name,
                ProjectName = project?.Name
            };

            stage.StageName = dto.StageName;
            stage.Status = SyncStatus(dto.Status, dto.IsCompleted);
            stage.IsCompleted = dto.IsCompleted;
            stage.Notes = dto.Notes;
            stage.ReportData = dto.ReportData;
            stage.StartDate = dto.StartDate ?? stage.StartDate;
            stage.EndDate = dto.EndDate;

            if (stage.IsCompleted && stage.EndDate == null)
                stage.EndDate = DateTime.UtcNow;

            _unitOfWork.ConstructionStages.UpdateConstructionStage(stage);
            await _unitOfWork.CompleteAsync();

            var newStageAr = StageNameHelper.ToArabic(stage.StageName);

            var newSnapshot = new
            {
                StageName = newStageAr,
                Status = stage.Status.ToString(),
                IsCompleted = stage.IsCompleted,
                Notes = stage.Notes,
                EndDate = stage.EndDate,
                ReportData = stage.ReportData,
                BuildingName = building?.Name,
                ProjectName = project?.Name
            };

            await _activityLogService.LogActivityAsync(
                "تعديل", "مرحلة بناء", id,
                $"تعديل مرحلة '{newStageAr}' — مبنى: {building?.Name ?? stage.BuildingId.ToString()} — مشروع: {project?.Name ?? "—"}",
                currentUserId,
                oldValues: oldSnapshot,
                newValues: newSnapshot);

            return true;
        }

        public async Task<bool> DeleteConstructionStageAsync(int id, int? currentUserId)
        {
            var stage = await _unitOfWork.ConstructionStages.GetConstructionStageByIdAsync(id);
            if (stage == null) return false;

            var building = await _unitOfWork.Buildings.GetBuildingByIdAsync(stage.BuildingId);
            var project = building != null
                ? await _unitOfWork.Projects.GetProjectByIdAsync(building.ProjectId)
                : null;

            var stageAr = StageNameHelper.ToArabic(stage.StageName);

            var oldSnapshot = new
            {
                StageName = stageAr,
                Status = stage.Status.ToString(),
                IsCompleted = stage.IsCompleted,
                Notes = stage.Notes,
                EndDate = stage.EndDate,
                ReportData = stage.ReportData,
                BuildingName = building?.Name,
                ProjectName = project?.Name
            };

            stage.IsDeleted = true;
            _unitOfWork.ConstructionStages.UpdateConstructionStage(stage);
            await _unitOfWork.CompleteAsync();

            await _activityLogService.LogActivityAsync(
                "حذف", "مرحلة بناء", id,
                $"حذف مرحلة '{stageAr}' — مبنى: {building?.Name ?? stage.BuildingId.ToString()} — مشروع: {project?.Name ?? "—"}",
                currentUserId,
                oldValues: oldSnapshot);

            return true;
        }

        private static StageStatus SyncStatus(StageStatus status, bool isCompleted)
        {
            if (isCompleted) return StageStatus.Completed;
            if (status == StageStatus.Completed) return StageStatus.InProgress;
            return status;
        }

        // baseUrl is passed in so this stays a pure static-like method with no hidden dependencies
        private static ConstructionStageResponseDto MapToResponse(ConstructionStage s, string baseUrl) => new()
        {
            Id = s.Id,
            BuildingId = s.BuildingId,
            BuildingName = s.Building?.Name ?? string.Empty,
            StageName = s.StageName,
            Status = s.Status,
            IsCompleted = s.IsCompleted,
            Notes = s.Notes,
            ReportData = s.ReportData,
            StartDate = s.StartDate,
            EndDate = s.EndDate,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            Images = s.Images?
                .Where(i => !i.IsDeleted)
                .Select(i => new StageImageResponseDto
                {
                    Id = i.Id,
                    ConstructionStageId = i.ConstructionStageId,
                    // Always return a full absolute URL so the frontend never needs to guess
                    ImageUrl = BuildImageUrl(i.ImageUrl, baseUrl),
                    Caption = i.Caption,
                    CreatedAt = i.CreatedAt
                }).ToList() ?? new()
        };

        // Ensures the URL is always absolute regardless of what was stored in the DB
        private static string BuildImageUrl(string storedPath, string baseUrl)
        {
            if (string.IsNullOrEmpty(storedPath)) return string.Empty;

            // Already absolute — return as-is
            if (storedPath.StartsWith("http://") || storedPath.StartsWith("https://"))
                return storedPath;

            // Relative path — prepend base URL
            return $"{baseUrl}/{storedPath.TrimStart('/')}";
        }
    }
}