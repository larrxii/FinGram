using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
    public class Test2Model : PageModel
    {
		private readonly AppDbContext _context;

		public Test2Model(AppDbContext context)
		{
			_context = context;
		}

		public List<Question> QuestionsWithAnswers { get; set; }

		[BindProperty]
		public Dictionary<int, string> Answers { get; set; } = new();

		public async Task OnGetAsync()
		{
			QuestionsWithAnswers = await _context.Questions
				.Where(q => q.TestId == 3)
				.Select(q => new Question
				{
					Id = q.Id,
					Text = q.Text,
					Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList()
				}).ToListAsync();
		}
	}
}
