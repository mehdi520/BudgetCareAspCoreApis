using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetAnalyzeUploadNewDrawResModel : BaseResponseModel
	{
        public DrawAnalyze data { get; set; }
    }
}
