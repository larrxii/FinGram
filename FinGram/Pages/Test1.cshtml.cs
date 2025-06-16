using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
	public class Test1Model : PageModel
	{
		private readonly AppDbContext _context;

		public Test1Model(AppDbContext context)
		{
			_context = context;
		}

		public List<Question> QuestionsWithAnswers { get; set; }

		[BindProperty]
		public Dictionary<int, string> Answers { get; set; } = new();

		public async Task OnGetAsync()
		{
			QuestionsWithAnswers = await _context.Questions
				.Where(q => q.TestId == 2)
				.Select(q => new Question
				{
					Id = q.Id,
					Text = q.Text,
					Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList()
				}).ToListAsync();
		}

		//public async Task<IActionResult> OnPostAsync()
		//{
		//	foreach (var entry in Answers)
		//	{
		//		var userAnswer = new UserTestResult
		//		{
		//			Id = entry.Key,
		//			CompletedAt = DateTime.UtcNow
		//		};

		//		_context.UserTestResults.Add(userAnswer);
		//	}

		//	await _context.SaveChangesAsync();
		//	return RedirectToPage("/TestSubmitted");
		//}
	}
}