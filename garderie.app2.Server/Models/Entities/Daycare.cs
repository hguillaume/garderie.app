using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace garderie.app2.Server.Models.Entities
{
    public class Daycare
    {
        public int id { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public required string name { get; set; }

        // Relationships can be added here, e.g., to Users or other entities
        public string? AspNetUserId { get; set; }
        //public User user { get; set; }

        public ICollection<Kid> Kids { get; set; }
    }

}
