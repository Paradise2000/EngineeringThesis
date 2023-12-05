namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class LocationSummaryDto
    {
        public int AttractionId { get; set; }
        public string Name { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }
    }
}
