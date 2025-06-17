using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
	public class Lesson_2Model : PageModel
	{
		private readonly AppDbContext _context;

		public Lesson_2Model(AppDbContext context)
		{
			_context = context;
		}

		public string Content { get; set; }

		public async Task OnGetAsync()
		{
			var lessons = await _context.Lessons.ToListAsync();

			foreach (var lesson in lessons)
			{
				Console.WriteLine($"Lesson: {lesson.Id} Ч {lesson.Title} Ч {lesson.Content}");
			}

			var lesson1 = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == 3);
			Content = lesson1?.Content ?? " онтент не найден";
		}
	}
}
