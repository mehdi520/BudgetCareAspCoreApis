using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos;
using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ReqModels;
using BudgetCareApis.Models.ReqModels.Users;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.User;

namespace BudgetCareApis.Services.services
{
	public interface IUserService
	{
		Task<BaseResponseModel> register(AuthenticationReqModel req);
		Task<LoginResModel> login(AuthenticationReqModel req);
		Task<GetUserResModel> getUserById(int user_id);


		Task<GetUserCatsResModel> getCatagories(int user_id);
		Task<BaseResponseModel> addUpdateCatagories(CategoryDataModel req);
		Task<BaseResponseModel> delCatagory(int cat_id );

		Task<GetTransResModel> getIncomes(GetTransReqModel req, int user_id);
		Task<BaseResponseModel> addUpdateIncome(TransDataModel req);
		Task<BaseResponseModel> delIncome(int incomeId);

		Task<GetTransResModel> getExpense(GetTransReqModel req, int user_id);
		Task<BaseResponseModel> addUpdateExpense(TransDataModel req);
		Task<BaseResponseModel> delExpense(int incomeId);
		Task<BaseResponseModel> updateProfile(UserViewModel req);
		Task<BaseResponseModel> changePassword(ChnagePassReqModel req);

		Task<GetTotalsResModel> getUserSummary(int userId);
		Task<GetGraphResModel> getUserGraphData(int userId);


	}
}
