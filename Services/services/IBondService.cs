using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.bond;

namespace BudgetCareApis.Services.services
{


	public interface IBondService
	{
		Task<GetRecordAvailYearsResModel> getRecordsAvailableYears();
		Task<GetDrawSchedulesByYearResModel> getDrawSchedulesByYear(int yearId);
		Task<GetAnalyzeUploadNewDrawResModel> analyzeUploadNewDrawResult(CreateNewDrawReqModel req);
		Task<BaseResponseModel> importDrawResult(int id);
		Task<GetDrawResultResModel> getDrawResult(int id);
	}
}
