using System.ComponentModel.DataAnnotations;

namespace garderie.app2.Server.Models.Entities
{
    public class User
    {
        public int id { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public required string name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public required string email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string password { get; set; }
    }
}
