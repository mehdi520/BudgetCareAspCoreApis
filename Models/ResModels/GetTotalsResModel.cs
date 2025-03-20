using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels
{
	public class GetTotalsResModel : BaseResponseModel
	{
        public TotalsDataModel data { get; set; }
    }
}
