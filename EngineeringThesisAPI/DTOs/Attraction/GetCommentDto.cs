namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class GetCommentDto
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
