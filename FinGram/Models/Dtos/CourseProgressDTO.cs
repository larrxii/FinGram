namespace FinGram.Models.Dtos
{
    public class CourseProgressDto
    {
        public string Title { get; set; }
        public int ProgressPercent { get; set; }
        public int MaxPoints { get; set; }
        public int CurrentPoints { get; set; }
        public bool HasCertificate { get; set; }
    }
}

