using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetBondsResModel : BaseResponseModel
	{
        public List<BondDataModel> data { get; set; }
    }
}
