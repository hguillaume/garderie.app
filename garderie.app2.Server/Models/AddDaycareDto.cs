namespace garderie.app2.Server.Models
{
    public class AddDaycareDto
    {
        public int id { get; set; }
        public required string name { get; set; }
        public int? user_id { get; set; } = null;
        //public required string email { get; set; }
        //public required string password { get; set; }
    }
}
