using Microsoft.EntityFrameworkCore;

namespace FinGram.Models
{
    public class FinGramUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Country { get; set; } // Обязательное поле
        public string Login { get; set; }
        public string UserPasswordHash { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<FinGramUser> FinGramUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinGramUser>().HasData(
                new FinGramUser
                {
                    Id = 1,
                    FirstName = "Arina",
                    LastName = "Trifonova",
                    BirthDate = DateTime.SpecifyKind(new DateTime(2000, 1, 1), DateTimeKind.Utc),
                    Country = "Russia",
                    Login = "arina",
                    UserPasswordHash = "hashedpassword"           
                },
                new FinGramUser
                {
                    Id = 2,
                    FirstName = "Camilla",
                    LastName = "Trifonova",
                    BirthDate = DateTime.SpecifyKind(new DateTime(2000, 1, 1), DateTimeKind.Utc),
                    Country = "Russia",
                    Login = "larrx",
                    UserPasswordHash = "hashedpassword"
                }
            );
        }

        public bool TestConnection()
        {
            try
            {
                return Database.CanConnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
        }
    }
}