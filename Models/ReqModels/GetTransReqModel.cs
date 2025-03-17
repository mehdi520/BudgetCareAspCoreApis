using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ReqModels
{
	public class GetTransReqModel : PageBaseReqModel
	{
        public DateOnly startDate { get; set; }
		public DateOnly endDate { get; set; }
		public int categoryId { get; set; }

	}
}
