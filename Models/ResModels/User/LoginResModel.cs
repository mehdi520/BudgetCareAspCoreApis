using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.User
{
	public class LoginResModel : BaseResponseModel
	{
        public UserViewModel data { get; set; }
        public string access_Token { get; set; }
    }
}
