using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels
{
	public class GetGraphResModel : BaseResponseModel
	{
        public List<GraphDataModel> data { get; set; }
    }
}
