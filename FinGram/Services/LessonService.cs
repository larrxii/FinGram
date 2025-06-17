using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Data;
using FinGram.Models;
using Microsoft.EntityFrameworkCore;

namespace FinGram.Services
{
	public class LessonService
	{
		private readonly AppDbContext _context;

		public LessonService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Lesson> GetLessonByIdAsync(int lessonId)
		{
			return await _context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
		}
	}
}