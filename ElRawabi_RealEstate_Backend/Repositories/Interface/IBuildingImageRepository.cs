using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IBuildingImageRepository
    {
        Task<IEnumerable<BuildingImage>> GetAllBuildingImagesAsync();
        Task<BuildingImage?> GetBuildingImageByIdAsync(int id);
        Task AddBuildingImageAsync(BuildingImage buildingImage);
        void UpdateBuildingImage(BuildingImage buildingImage);
        void DeleteBuildingImage(BuildingImage buildingImage);
        Task SaveChangesAsync();
        Task<IEnumerable<BuildingImage>> GetBuildingImagesByBuildingIdAsync(int buildingId);
    }
}
