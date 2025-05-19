namespace BudgetCareApis.Models.Dtos.bond
{
	public class DrawSchedulesByYearDataModel
	{
		public int Id { get; set; }

		public int YearId { get; set; }

		public string Amount { get; set; } = null!;

		public bool IsPremium { get; set; }

		public string Day { get; set; } = null!;

		public DateTime Date { get; set; }

		public string Place { get; set; } = null!;

		public bool IsAnnounced { get; set; }
	}
}
