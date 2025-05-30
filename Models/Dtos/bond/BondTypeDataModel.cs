namespace BudgetCareApis.Models.Dtos.bond
{
	public class BondTypeDataModel
	{
		public int TypeId { get; set; }

		public string BondType { get; set; } = null!;

		public bool IsPermium { get; set; }
	}
}
