using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetUserWonBondsResModel : BaseResponseModel
	{
        public List<WonBondDataModel> data { get; set; }
    }
}
