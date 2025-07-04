using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinGram.Models;
using FinGram.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FinGram.Pages
{
	public class Test2Model : PageModel
	{
		private readonly TestService _testService;
		private readonly UserManager<User> _userManager;

		public Test2Model(TestService testService, UserManager<User> userManager)
		{
			_testService = testService;
			_userManager = userManager;
		}

		public List<Question> QuestionsWithAnswers { get; set; }

		[BindProperty]
		public Dictionary<int, int> Answers { get; set; } = new();

		public int? Score { get; set; } // ��� ������ ����������
		public int TotalQuestions { get; set; }

		public async Task OnGetAsync()
		{
			QuestionsWithAnswers = await _testService.GetQuestionsByTestIdAsync(3);
			TotalQuestions = QuestionsWithAnswers.Count;
		}

		public async Task<IActionResult> OnPostAsync()
		{
			QuestionsWithAnswers = await _testService.GetQuestionsByTestIdAsync(3);
			TotalQuestions = QuestionsWithAnswers.Count;

			Score = await _testService.EvaluateTestAsync(3, Answers);

			// ���������� ���� � ��
			var user = await _userManager.GetUserAsync(User);
			if (user != null)
			{
				await _testService.SaveTestResultAsync(user.Id, 3, Score.Value);
			}

			return Page();
		}
	}
}