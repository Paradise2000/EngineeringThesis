namespace EngineeringThesisAPI.Entities
{
    public class Attraction
    {
        public int Id { get; set; }
        public string City { get; set; }
        public TimeSpan Duration { get; set; }
        public float Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }

        public int MainPhotoId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<FilePath> Photos { get; set; }

    }
}
