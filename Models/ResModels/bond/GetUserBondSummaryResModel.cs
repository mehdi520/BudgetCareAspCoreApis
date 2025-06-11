using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetUserBondSummaryResModel : BaseResponseModel
	{
        public UserBondSummaryDataModel data { get; set; }
    }

	public class UserBondSummaryDataModel {
        public int TotalBond { get; set; }
        public int TotalWorth { get; set; }

        public List<UserBondCountByType> bonds { get; set; }

    }

	public class UserBondCountByType {
        public string bondType { get; set; }
		public int bondCount { get; set; }
		public int bondWorth { get; set; }
	}
}
