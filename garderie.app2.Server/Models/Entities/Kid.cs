using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace garderie.app2.Server.Models.Entities
{
    public class Kid
    {
        public int id { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public required string name { get; set; }

        // Relationships can be added here, e.g., to Users or other entities
        public int daycareId { get; set; }
        //public Daycare daycare { get; set; }
    }

}
