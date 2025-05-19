using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetDrawSchedulesByYearResModel : BaseResponseModel
	{
        public List<DrawSchedulesByYearDataModel> data { get; set; }
    }
}
