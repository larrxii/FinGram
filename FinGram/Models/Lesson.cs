namespace FinGram.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public Course Course { get; set; }
        public ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();
        public ICollection<Test> Tests { get; set; } = new List<Test>();

    }
}
