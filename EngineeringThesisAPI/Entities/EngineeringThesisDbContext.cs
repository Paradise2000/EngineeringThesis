using Microsoft.EntityFrameworkCore;

namespace EngineeringThesisAPI.Entities
{
    public class EngineeringThesisDbContext : DbContext
    {
        public EngineeringThesisDbContext(DbContextOptions<EngineeringThesisDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id= 1,
                    Name = "Zabytki",
                },
                new Category
                {
                    Id = 2,
                    Name = "Gastronomia",
                },
                new Category
                {
                    Id = 3,
                    Name = "Przyroda",
                },
                new Category
                {
                    Id = 4,
                    Name = "Muzeum",
                },
                new Category
                {
                    Id = 5,
                    Name = "Adrenalina",
                },
                new Category
                {
                    Id = 6,
                    Name = "Inne",
                }
            );

            modelBuilder.Entity<User>()
                .HasMany(u => u.Attractions)
                .WithOne(a => a.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attraction>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.Attraction)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
