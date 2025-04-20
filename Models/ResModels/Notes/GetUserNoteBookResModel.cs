using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.Notes
{
	public class GetUserNoteBookResModel : BaseResponseModel
	{
		public List<NoteBookModel> data { get; set; }
	}
	
}
