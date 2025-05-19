namespace BudgetCareApis.Models.Dtos.bond
{
	public class DrawWinsBondDataModel
	{
		public int Id { get; set; }

		public int DrawId { get; set; }

		public string BoundNo { get; set; } = null!;

		public int Position { get; set; }
	}
}
