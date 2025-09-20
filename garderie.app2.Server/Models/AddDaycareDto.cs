namespace garderie.app2.Server.Models
{
    public class AddDaycareDto
    {
        public int id { get; set; }
        public required string name { get; set; }
        public string? AspNetUserId { get; set; }
    }
}
