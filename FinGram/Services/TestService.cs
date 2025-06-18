using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;
namespace FinGram.Services
{
	public class TestService
	{
		private readonly AppDbContext _context;

		public TestService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Question>> GetQuestionsByTestIdAsync(int testId)
		{
			return await _context.Questions
				.Where(q => q.TestId == testId)
				.Select(q => new Question
				{
					Id = q.Id,
					Text = q.Text,
					Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList()
				}).ToListAsync();
		}

		// для тестов
		// подсчет баллов
		public async Task<int> EvaluateTestAsync(int testId, Dictionary<int, int> userAnswers)
		{
			var questions = await _context.Questions
				.Where(q => q.TestId == testId)
				.Include(q => q.Answers)
				.ToListAsync();

			int score = 0;

			foreach (var question in questions)
			{
				var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
				if (correctAnswer != null && userAnswers.TryGetValue(question.Id, out var selectedAnswerId))
				{
					if (selectedAnswerId == correctAnswer.Id)
					{
						score++;
					}
				}
			}

			return score;
		}


		// сохранение быллов
		public async Task SaveTestResultAsync(int userId, int testId, int score)
		{
			var existing = await _context.UserTestResults
				.FirstOrDefaultAsync(r => r.UserId == userId && r.TestId == testId);

			if (existing != null)
			{
				existing.Score = score;
				existing.CompletedAt = DateTime.UtcNow;
			}
			else
			{
				var result = new UserTestResult
				{
					UserId = userId,
					TestId = testId,
					Score = score,
					CompletedAt = DateTime.UtcNow
				};
				await _context.UserTestResults.AddAsync(result);
			}

			var test = await _context.Tests
			.Include(t => t.Course)
			.FirstOrDefaultAsync(t => t.Id == testId);

			if (test != null && test.CourseId != null)
			{
				var lessonsInCourse = await _context.Lessons
					.Where(l => l.CourseId == test.CourseId)
					.ToListAsync();

				var passedLessonIds = await _context.UserLessons
					.Where(ul => ul.UserId == userId)
					.Select(ul => ul.LessonId)
					.ToListAsync();

				var newLessons = lessonsInCourse
					.Where(l => !passedLessonIds.Contains(l.Id))
					.ToList();

				foreach (var lesson in newLessons)
				{
					_context.UserLessons.Add(new UserLesson
					{
						UserId = userId,
						LessonId = lesson.Id
					});
				}
			}

			await _context.SaveChangesAsync();
		}

	}
}
