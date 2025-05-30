using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos.bond;

namespace BudgetCareApis.Models.ReqModels.bond
{
	public class CreateNewDrawReqModel
	{
		public int draw_id { get; set; }

		public DateTime draw_date { get; set; }
        public int draw_no { get; set; }
		public string first_prize_worth { get; set; }
		public string second_prize_worth { get; set; }
		public string third_prize_worth { get; set; }

		public IFormFile file { get; set; }

        public List<DrawWinsBondDataModel>? bonds { get; set; }
    }
}
