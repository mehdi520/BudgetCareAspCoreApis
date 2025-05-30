using BudgetCareApis.Data.Entities;

namespace BudgetCareApis.Models.Dtos.bond
{
	public class DrawDataModel
	{
		public int DrawId { get; set; }

		public int BondTypeId { get; set; }
        public string BondType { get; set; }
		public string? Place { get; set; }

		public string? Day { get; set; }
		public DateTime DrawDate { get; set; }

		public int? DrawNo { get; set; }

		public string? FirstPrizeWorth { get; set; }

		public string? SecondPrizeWorth { get; set; }

		public string? ThirdPrizeWorth { get; set; }

		public bool IsResultAnnounced { get; set; }

	}
}
