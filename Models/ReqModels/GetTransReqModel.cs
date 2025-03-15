using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ReqModels
{
	public class GetTransReqModel : PageBaseReqModel
	{
        public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public int categoryId { get; set; }

	}
}
