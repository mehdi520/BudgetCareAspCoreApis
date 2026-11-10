using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.Notes
{
	public class GetUserNoteBookResModel : PagedResponseModel
	{
		public List<NoteBookModel> data { get; set; } = new();
	}
}
