namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class UpdateAttractionDto
    {
        public int AttractionId { get; set; }
        public TimeSpan Duration { get; set; }
        public string City { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }
        public List<string> ImagesPaths { get; set; }
        public string MainImagePath { get; set; }
    }
}
