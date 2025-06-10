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
		Task<GetBondsResModel> getUserBonds(int userId,int typeId);
		Task<BaseResponseModel> addUpdateUserBond(int userId, BondDataModel req);
		Task<BaseResponseModel> deleteUserBond(int userId, int bondId);
		Task<BaseResponseModel> DrawWinCheckSyncByDraw(int drawId);
		Task<GetUserWonBondsResModel> GetUserWonBonds(int userId, string status);
		Task<BaseResponseModel> UpdateUserWonBondStatus(int userId, string status, int wonId);

		




	}
}
