using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.User
{
	public class GetUserResModel : BaseResponseModel
	{
        public  UserViewModel data { get; set; }
    }
}
