using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetDrawsResModel : BaseResponseModel
	{
        public List<DrawDataModel> data { get; set; }
    }
}
