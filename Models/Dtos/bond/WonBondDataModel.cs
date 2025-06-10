using BudgetCareApis.Data.Entities;

namespace BudgetCareApis.Models.Dtos.bond
{
	public class WonBondDataModel
	{
		public int WonId { get; set; }

		public string Status { get; set; } = null!;

		public int BondId { get; set; }

		public int DrawId { get; set; }

		public int UserId { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public int Position { get; set; }

		public  BondDataModel Bond { get; set; } = null!;

		public  DrawDataModel Draw { get; set; } = null!;

	}
}
