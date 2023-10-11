namespace EngineeringThesisAPI.Entities
{
    public class Attraction
    {
        public int Id { get; set; }
        public string City { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
