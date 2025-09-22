using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace garderie.app2.Server.Models.Entities
{
    public class Answer
    {
        public int id { get; set; }
        
        //[Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        //public required string name { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }

        // Relationships can be added here, e.g., to Users or other entities
        public int kidId { get; set; }
        public int questionId { get; set; }
        public int daycareId { get; set; }
        //public Daycare daycare { get; set; }
    }

}
