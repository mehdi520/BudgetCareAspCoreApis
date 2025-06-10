using BudgetCareApis.Data.Entities;

namespace BudgetCareApis.Models.Dtos.bond
{
	public class BondDataModel
	{
		public int BondId { get; set; }

		public string BondNumber { get; set; } = null!;

		public int BondType { get; set; }

		public DateTime? CreatedAt { get; set; }

		public bool IsDeleted { get; set; }

		public int UserId { get; set; }
	}
}
