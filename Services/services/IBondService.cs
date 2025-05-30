using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.bond;
using DocumentFormat.OpenXml.Drawing;

namespace BudgetCareApis.Services.services
{


	public interface IBondService
	{
		Task<GetBondTypesResModel> getBondTypes();
		Task<GetDrawsResModel> getDrawsByBondType(int bondType);
		Task<BaseResponseModel> addUpdateDrawsByBondType(DrawDataModel req);
		Task<GetAnalyzeUploadNewDrawResModel> analyzeUploadNewDrawResult(CreateNewDrawReqModel req);
		Task<BaseResponseModel> importDrawResult(int id);
		Task<GetDrawResultResModel> getDrawResult(int id);
	}
}
