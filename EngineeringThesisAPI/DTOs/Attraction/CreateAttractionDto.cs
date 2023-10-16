namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class CreateAttractionDto
    {
        public string City { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }
    }
}
