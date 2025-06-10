namespace FinGram.Models
{
    public class UserTestResult
    {
        public int Id { get; set; }
        public int TestId { get; set; } 
        public int UserId { get; set; } 
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;

        public Test Test { get; set; }
        public User User { get; set; }
    }
}
