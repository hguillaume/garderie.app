namespace garderie.app2.Server.Models
{
    public class AddQuestionDto
    {
        public int id { get; set; }
        public required string name { get; set; }
        public string? description { get; set; }
        public int daycareId { get; set; }
    }
}
