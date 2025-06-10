namespace FinGram.Models
{
    public class UserLesson
    {
        public int UserLessonsId { get; set; } 
        public int UserId { get; set; } 
        public int LessonId { get; set; } 

        public User User { get; set; }
        public Lesson Lesson { get; set; }
    }
}
