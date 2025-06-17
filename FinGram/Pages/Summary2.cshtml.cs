using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Pages
{
	public class Summary2Model : PageModel
	{
		private readonly AppDbContext _context;

		public Summary2Model(AppDbContext context)
		{
			_context = context;
		}

		public string Content { get; set; }
		public List<Course> CoursesList { get; set; }

		public async Task OnGetAsync()
		{
			CoursesList = await _context.Courses.ToListAsync();

			foreach (var course in CoursesList)
			{
				Console.WriteLine($"Course: {course.Description}");
			}

			var desc = await _context.Courses.FirstOrDefaultAsync(l => l.Id == 3);
			Content = desc?.Description ?? "Контент не найден";
		}
	}
}