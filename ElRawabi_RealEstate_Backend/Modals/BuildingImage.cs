namespace ElRawabi_RealEstate_Backend.Modals
{
    public class BuildingImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public Building Building { get; set; }
        public int BuildingId { get; set; }
    }
}
