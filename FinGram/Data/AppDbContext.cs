using FinGram.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserLesson> UserLessons { get; set; }
        public DbSet<UserTestResult> UserTestResults { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for UserLesson
            modelBuilder.Entity<UserLesson>()
                .HasKey(ul => new { ul.UserLessonsId, ul.UserId, ul.LessonId });

            // Lesson -> Course (one-to-many)
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseId);

            // Test -> Course (one-to-many, optional)
            modelBuilder.Entity<Test>()
                .HasOne(t => t.Course)
                .WithMany(c => c.Tests)
                .HasForeignKey(t => t.CourseId)
                .IsRequired(false);

            // Test -> Lesson (one-to-many, optional)
            modelBuilder.Entity<Test>()
                .HasOne(t => t.Lesson)
                .WithMany(l => l.Tests)
                .HasForeignKey(t => t.LessonId)
                .IsRequired(false);

            // Test CHECK constraint
            modelBuilder.Entity<Test>()
                .HasCheckConstraint("check_lesson_or_course",
                    "\"LessonId\" IS NOT NULL AND \"CourseId\" IS NULL OR \"LessonId\" IS NULL AND \"CourseId\" IS NOT NULL");

            // Question -> Test (one-to-many)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Test)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TestId);

            // Answer -> Question (one-to-many)
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            // UserLesson -> User (many-to-one)
            modelBuilder.Entity<UserLesson>()
                .HasOne(ul => ul.User)
                .WithMany(u => u.UserLessons)
                .HasForeignKey(ul => ul.UserId);

            // UserLesson -> Lesson (many-to-one)
            modelBuilder.Entity<UserLesson>()
                .HasOne(ul => ul.Lesson)
                .WithMany(l => l.UserLessons)
                .HasForeignKey(ul => ul.LessonId);

            // UserTestResult -> Test (many-to-one)
            modelBuilder.Entity<UserTestResult>()
                .HasOne(utr => utr.Test)
                .WithMany(t => t.UserTestResults)
                .HasForeignKey(utr => utr.TestId);

            // UserTestResult -> User (many-to-one)
            modelBuilder.Entity<UserTestResult>()
                .HasOne(utr => utr.User)
                .WithMany(u => u.UserTestResults)
                .HasForeignKey(utr => utr.UserId);

            // Certificate -> User (one-to-many)
            modelBuilder.Entity<Certificate>()
                .HasOne(c => c.User)
                .WithMany(u => u.Certificates)
                .HasForeignKey(c => c.UserId);
        }
    }
}