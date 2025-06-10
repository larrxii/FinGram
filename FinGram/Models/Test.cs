namespace FinGram.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? LessonId { get; set; }
        public string Title { get; set; }
        public bool IsFinal { get; set; }

        public Course Course { get; set; }
        public Lesson Lesson { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<UserTestResult> UserTestResults { get; set; } = new List<UserTestResult>();
    }
}
