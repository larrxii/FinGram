using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
    public class Lesson1_1Model : PageModel
    {
		private readonly AppDbContext _context;

		public Lesson1_1Model(AppDbContext context)
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

			var lesson1 = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == 2);
			Content = lesson1?.Content ?? " онтент не найден";
		}
    }
}
