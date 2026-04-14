using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Modals
{
    public class Role
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<User> Users { get; set; } = new List<User>();
    }
}
