using BudgetCareApis.Models.Dtos;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels
{
	public class GetTransResModel : BaseResponseModel
	{
        public int TotalPage { get; set; }
		public decimal totalAmount { get; set; }
		public List<TransDataModel> data { get; set; }
    }
}
