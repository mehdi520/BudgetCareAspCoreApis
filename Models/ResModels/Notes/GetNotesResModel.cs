using BudgetCareApis.Models.Dtos;
using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.Notes
{
	public class GetNotesResModel : BaseResponseModel
	{
		public int TotalPage { get; set; }
		public List<NoteDataModel> data { get; set; }
	}

}
