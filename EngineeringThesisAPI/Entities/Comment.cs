namespace EngineeringThesisAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        public User User { get; set; }
        public Attraction Attraction { get; set; }
    }
}
