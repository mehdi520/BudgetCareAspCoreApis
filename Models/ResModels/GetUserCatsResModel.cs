using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.ResModels.Base;
using System.Collections;

namespace BudgetCareApis.Models.ResModels
{
	public class GetUserCatsResModel : BaseResponseModel
	{
        public List<CategoryDataModel> data { get; set; }
    }
}
