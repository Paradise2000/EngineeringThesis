namespace EngineeringThesisAPI.Entities
{
    public class FilePath
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime AddTime { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? AttractionId { get; set; }
        public Attraction Attraction { get; set; }
    }
}
