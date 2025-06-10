namespace FinGram.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string CertificateUrl { get; set; }

        public User User { get; set; }

    }
}
