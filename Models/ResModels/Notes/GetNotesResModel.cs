using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.Notes
{
	public class GetNotesResModel : PagedResponseModel
	{
		public List<NoteDataModel> data { get; set; } = new();
	}

	public class GetNoteResModel : BaseResponseModel
	{
		public NoteDataModel? data { get; set; }
	}

	public class GetAttachmentsResModel : BaseResponseModel
	{
		public List<NoteAttachmentModel> data { get; set; } = new();
	}

	/// <summary>All notes of a notebook (with attachments) for PDF export.</summary>
	public class GetNoteBookExportResModel : BaseResponseModel
	{
		public NoteBookModel? noteBook { get; set; }
		public List<NoteDataModel> data { get; set; } = new();
	}
}
