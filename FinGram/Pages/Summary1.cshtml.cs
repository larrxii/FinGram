using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;
using FinGram.Services;

namespace FinGram.Pages
{
	public class Summary1Model : PageModel
	{
		private readonly CourseService _courseService;

		public Summary1Model(CourseService courseService)
		{
			_courseService = courseService;
		}

		public string Content { get; set; }
		public List<Course> CoursesList { get; set; }

		public async Task OnGetAsync()
		{
			CoursesList = await _courseService.GetAllCoursesAsync();

			foreach (var course in CoursesList)
			{
				Console.WriteLine($"Course: {course.Description}");
			}

			Content = await _courseService.GetCourseDescriptionByIdAsync(2);
		}
	}
}