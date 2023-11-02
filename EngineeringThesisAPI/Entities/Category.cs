namespace EngineeringThesisAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Attraction> Attractions { get; set; }
    }
}
