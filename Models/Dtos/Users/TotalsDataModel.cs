namespace BudgetCareApis.Models.Dtos.Users
{
	public class TotalsDataModel
	{
        public decimal totalThisMonthIncome { get; set; }
		public decimal totalThisMonthExpense { get; set; }
		public decimal totalThisYearIncome { get; set; }
		public decimal totalThisYearExpense { get; set; }

	}
}
