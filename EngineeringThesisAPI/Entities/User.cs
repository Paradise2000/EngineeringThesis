namespace EngineeringThesisAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }

        public ICollection<Attraction> Attractions { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
