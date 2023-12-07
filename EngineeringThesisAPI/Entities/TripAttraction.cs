namespace EngineeringThesisAPI.Entities
{
    public class TripAttraction
    {
        public int Id { get; set; }

        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
