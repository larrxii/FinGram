using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
	public class TestFinalModel : PageModel
	{
		private readonly AppDbContext _context;

		public TestFinalModel(AppDbContext context)
		{
			_context = context;
		}

		public List<Question> QuestionsWithAnswers { get; set; }

		[BindProperty]
		public Dictionary<int, string> Answers { get; set; } = new();

		public async Task OnGetAsync()
		{
			QuestionsWithAnswers = await _context.Questions
				.Where(q => q.TestId == 5)
				.Select(q => new Question
				{
					Id = q.Id,
					Text = q.Text,
					Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList()
				}).ToListAsync();
		}
	}
}
