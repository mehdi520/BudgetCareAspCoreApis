using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetAnalyzeUploadNewDrawResModel : BaseResponseModel
	{

		public int totalBonds { get; set; }
		public int totalFirst { get; set; }
		public int totalSecond { get; set; }
		public int totalThird { get; set; }

		public DrawAnalyze data { get; set; }
    }
}
