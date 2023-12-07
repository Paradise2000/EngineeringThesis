namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class GetAttractionsDto
    {
        public string City { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainImagePath { get; set; }
    }
}
