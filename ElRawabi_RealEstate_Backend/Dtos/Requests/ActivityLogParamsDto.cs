namespace ElRawabi_RealEstate_Backend.Dtos.Requests
{
    public class ActivityLogParamsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
