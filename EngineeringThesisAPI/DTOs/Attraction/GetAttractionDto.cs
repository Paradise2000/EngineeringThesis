namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class GetAttractionDto
    {
        public string City { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }
        public double AvgReview { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOf5StarReviews { get; set; }
        public int NumberOf4StarReviews { get; set; }
        public int NumberOf3StarReviews { get; set; }
        public int NumberOf2StarReviews { get; set; }
        public int NumberOf1StarReviews { get; set; }
        public GetCommentDto UserComment { get; set; }
        public List<string> ImagePaths { get; set; }
        public string MainImagePath { get; set; }
        public bool isUserAttracion { get; set; }
    }
}
