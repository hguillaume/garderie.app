namespace garderie.app2.Server.Models
{
    public class AddAnswerDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int daycareId { get; set; }
        public int kidId { get; set; }
        public int questionId { get; set; }
    }
}
