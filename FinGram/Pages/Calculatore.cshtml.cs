using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinGram.Pages
{
	public class CalculatoreModel : PageModel
	{
		public class InputModel
		{
			[BindProperty]
			public decimal MainIncome { get; set; }

			[BindProperty]
			public decimal AdditionalIncome { get; set; }

			[BindProperty]
			public decimal HousingExpense { get; set; }

			[BindProperty]
			public decimal FoodExpense { get; set; }

			[BindProperty]
			public decimal TransportExpense { get; set; }

			[BindProperty]
			public decimal CommunicationExpense { get; set; }

			[BindProperty]
			public decimal SavingGoalPerMonth { get; set; }
		}

		[BindProperty]
		public InputModel Input { get; set; } = new();

		public decimal MonthlyBudgetLimit { get; set; }

		public bool ResultVisible { get; set; }

		public void OnPost()
		{
			var totalIncome = Input.MainIncome + Input.AdditionalIncome;
			var totalExpenses = Input.HousingExpense + Input.FoodExpense + Input.TransportExpense + Input.CommunicationExpense + Input.SavingGoalPerMonth;

			MonthlyBudgetLimit = totalIncome - totalExpenses;
			ResultVisible = true;
		}
	}
}
