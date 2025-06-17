using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Models;
using FinGram.Services;
using System;
using System.Threading.Tasks;
using FinGram.Services;

namespace FinGram.Pages
{
	public class Lesson1_1Model : PageModel
	{
		private readonly LessonService _lessonService;

		public Lesson1_1Model(LessonService lessonService)
		{
			_lessonService = lessonService;
		}

		public string Content { get; set; }

		public async Task OnGetAsync()
		{
			var lesson1 = await _lessonService.GetLessonByIdAsync(2);
			Content = lesson1?.Content ?? "Контент не найден";
		}
	}
}
