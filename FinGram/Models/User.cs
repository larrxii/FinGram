using Microsoft.AspNetCore.Identity;
namespace FinGram.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Login { get; set; }
        //public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();
        public ICollection<UserTestResult> UserTestResults { get; set; } = new List<UserTestResult>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }
}