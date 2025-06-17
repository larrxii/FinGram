function addIncomeField() {
	const container = document.getElementById("income-section");
	const input = document.createElement("input");
	input.type = "number";
	input.placeholder = "Доп. доход, ₽";
	input.className = "form-control income-field mt-2";
	container.appendChild(input);
}

function addExpenseField() {
	const container = document.getElementById("expenses-section");
	const input = document.createElement("input");
	input.type = "number";
	input.placeholder = "Расход, ₽";
	input.className = "form-control expense-field mt-2";
	container.appendChild(input);
}

function calculateBudget() {
	let incomeSum = 0;
	document.querySelectorAll(".income-field").forEach(field => {
		incomeSum += parseFloat(field.value) || 0;
	});

	let expenseSum = 0;
	document.querySelectorAll(".expense-field").forEach(field => {
		expenseSum += parseFloat(field.value) || 0;
	});

	const savings = parseFloat(document.getElementById("savings-input").value) || 0;
	const totalOutgoings = expenseSum + savings;

	const warningEl = document.getElementById("warning-message");

	if (totalOutgoings > incomeSum) {
		warningEl.innerText = "Упс! Похоже ваши расходы превышают доходы";
		warningEl.style.display = "block";
		document.getElementById("result-section").style.display = "none";
		return;
	} else {
		warningEl.style.display = "none";
	}

	const monthlyLimit = incomeSum - totalOutgoings;
	const dailyBudget = monthlyLimit / 30;
	const monthlySavings = savings;
	const yearlySavings = savings * 12;

	document.getElementById("daily-budget").innerText = dailyBudget.toFixed(2);
	document.getElementById("monthly-limit").innerText = monthlyLimit.toFixed(2);
	document.getElementById("monthly-savings").innerText = monthlySavings.toFixed(2);
	document.getElementById("yearly-savings").innerText = yearlySavings.toFixed(2);

	document.getElementById("result-section").style.display = "block";
}
