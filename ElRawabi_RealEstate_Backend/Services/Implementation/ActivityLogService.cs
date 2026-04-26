using AutoMapper;
using ElRawabi_RealEstate_Backend.Dtos.Requests;
using ElRawabi_RealEstate_Backend.Dtos.Responses;
using ElRawabi_RealEstate_Backend.DTOs.Responses;
using ElRawabi_RealEstate_Backend.Modals;
using ElRawabi_RealEstate_Backend.Repositories.Interface;
using ElRawabi_RealEstate_Backend.Services.Interface;
using System.Text.Json;

namespace ElRawabi_RealEstate_Backend.Services.Implementation
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private static readonly TimeZoneInfo SaudiTz =
            TimeZoneInfo.FindSystemTimeZoneById(
                Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? "Arab Standard Time"
                    : "Asia/Riyadh"
            );

        private static readonly Dictionary<string, string> FieldNames = new()
        {
            { "firstName", "الاسم الأول" },
            { "lastName", "الاسم الأخير" },
            { "email", "البريد الإلكتروني" },
            { "phoneNumber", "رقم الهاتف" },
            { "roleId", "الدور الوظيفي" },
            { "roleName", "الدور الوظيفي" },
            { "unitId", "الوحدة" },
            { "buyerId", "العميل" },
            { "status", "الحالة" },
            { "bookingDate", "تاريخ الحجز" },
            { "unitNumber", "رقم الوحدة" },
            { "floorId", "الدور" },
            { "area", "المساحة" },
            { "price", "السعر" },
            { "name", "الاسم" },
            { "projectId", "المشروع" },
            { "totalUnits", "إجمالي الوحدات" },
            { "location", "الموقع" },
            { "floorNumber", "رقم الدور" },
            { "buildingId", "المبنى" },
            { "buildingName", "المبنى" },
            { "projectName", "المشروع" },
            { "stageName", "اسم المرحلة" },
            { "isCompleted", "مكتملة" },
            { "notes", "ملاحظات" },
            { "endDate", "تاريخ الانتهاء" },
            { "startDate", "تاريخ البدء" },
            { "reportData", "تقرير المرحلة" },
            { "imageUrl", "رابط الصورة" },
            { "caption", "وصف الصورة" },
            { "description", "الوصف" },
            { "isActive", "نشط" },
        };

        // ترجمة الـ Status values
        private static readonly Dictionary<string, string> StatusValues = new()
        {
            { "Available", "متاح" },
            { "Reserved", "محجوز" },
            { "Sold", "مباع" },
            { "Closed", "مغلق" },
            { "Pending", "قيد الانتظار" },
            { "Confirmed", "مؤكد" },
            { "Cancelled", "ملغي" },
            { "InProgress", "جاري التنفيذ" },
            { "Completed", "مكتمل" },
            { "True", "نعم" },
            { "False", "لا" },
        };

        // ترجمة الـ RoleId لاسم
        private static readonly Dictionary<string, string> RoleNames = new()
        {
            { "1", "مدير النظام" },
            { "2", "مدير الحجوزات" },
            { "3", "مهندس الموقع" },
        };

        public async Task<IEnumerable<ActivityLogResponseDto>> GetAllActivityLogsAsync() =>
            _mapper.Map<IEnumerable<ActivityLogResponseDto>>(await _unitOfWork.ActivityLogs.GetAllActivityLogsAsync());

        public async Task<ActivityLogResponseDto?> GetActivityLogByIdAsync(int id) =>
            _mapper.Map<ActivityLogResponseDto>(await _unitOfWork.ActivityLogs.GetActivityLogByIdAsync(id));

        public async Task LogActivityAsync(string action, string entity, int entityId, string? details, int? userId, object? oldValues = null, object? newValues = null)
        {
            var saudiTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, SaudiTz);

            var log = new ActivityLog
            {
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Details = details,
                UserId = userId,
                Timestamp = saudiTime,
                OldValues = oldValues != null ? TranslateValues(JsonSerializer.Serialize(oldValues)) : null,
                NewValues = newValues != null ? TranslateValues(JsonSerializer.Serialize(newValues)) : null
            };

            await _unitOfWork.ActivityLogs.AddActivityLogAsync(log);
            await _unitOfWork.CompleteAsync();
        }

        private string TranslateValues(string json)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                var result = new Dictionary<string, string>();

                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    // ترجمة اسم الـ field
                    var fieldName = FieldNames.TryGetValue(prop.Name.ToLower(), out var translatedField)
                        ? translatedField
                        : prop.Name;

                    // ترجمة القيمة
                    var rawValue = prop.Value.ToString();
                    string translatedValue;

                    // لو roleId حوله لاسم الدور
                    if (prop.Name.ToLower() == "roleid")
                    {
                        translatedValue = RoleNames.TryGetValue(rawValue, out var roleName)
                            ? roleName
                            : $"دور #{rawValue}";
                    }
                    // لو status حوله للعربي
                    else if (StatusValues.TryGetValue(rawValue, out var statusTranslated))
                    {
                        translatedValue = statusTranslated;
                    }
                    // لو تاريخ نسقه
                    else if (DateTime.TryParse(rawValue, out var date))
                    {
                        translatedValue = date.ToString("yyyy/MM/dd hh:mm tt");
                    }
                    // لو رقم كبير وفيه Id في الاسم
                    else if (prop.Name.ToLower().EndsWith("id") && int.TryParse(rawValue, out var idVal))
                    {
                        translatedValue = $"#{idVal}";
                    }
                    else
                    {
                        translatedValue = rawValue;
                    }

                    result[fieldName] = translatedValue;
                }

                return JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }
            catch
            {
                return json;
            }
        }

        public async Task<PagedLogResponseDto> GetFilteredActivityLogsAsync(ActivityLogParamsDto filter)
        {
            var result = await _unitOfWork.ActivityLogs.GetFilteredActivityLogsAsync(filter);

            return new PagedLogResponseDto
            {
                Items = _mapper.Map<IEnumerable<ActivityLogResponseDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                CreateCount = result.CreateCount,
                UpdateCount = result.UpdateCount,
                DeleteCount = result.DeleteCount
            };
        }
    }
}