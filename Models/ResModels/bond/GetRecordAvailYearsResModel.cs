using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetRecordAvailYearsResModel : BaseResponseModel
	{
        public List<RecordAvailYearsDataModel> data { get; set; }
    }
}
