namespace garderie.app2.Server.Models
{
    public class AddKidDto
    {
        public int id { get; set; }
        public required string name { get; set; }
        public int daycareId { get; set; }
    }
}
