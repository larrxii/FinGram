using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
	public class Lesson2_1Model : PageModel
	{
		private readonly AppDbContext _context;

		public Lesson2_1Model(AppDbContext context)
		{
			_context = context;
		}

		public string Content { get; set; }

		public async Task OnGetAsync()
		{
			var lessons = await _context.Lessons.ToListAsync();

			foreach (var lesson in lessons)
			{
				Console.WriteLine($"Lesson: {lesson.Id} � {lesson.Title} � {lesson.Content}");
			}

			var lesson1 = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == 5);
			Content = lesson1?.Content ?? "������� �� ������";
		}
	}
}
