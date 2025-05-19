using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetDrawResultResModel : BaseResponseModel
	{
        public CreateNewDrawReqModel data { get; set; }
    }
}
