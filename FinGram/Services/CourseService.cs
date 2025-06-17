using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Services
{
	public class CourseService
	{
		private readonly AppDbContext _context;

		public CourseService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Course>> GetAllCoursesAsync()
		{
			return await _context.Courses.ToListAsync();
		}

		public async Task<string> GetCourseDescriptionByIdAsync(int courseId)
		{
			var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
			return course?.Description ?? "Контент не найден";
		}
	}
}