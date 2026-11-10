using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ReqModels.Notes
{
	public class GetNotesReqModel : PageBaseReqModel
	{
		/// <summary>Notebook id. 0 = search across all notebooks.</summary>
		public int categoryId { get; set; }

		/// <summary>Matches note title or details.</summary>
		public string? search { get; set; }
	}

	public class GetNoteBooksReqModel : PageBaseReqModel
	{
		/// <summary>Matches notebook title.</summary>
		public string? search { get; set; }
	}

	public class RenameAttachmentReqModel
	{
		public int attachmentId { get; set; }
		public string fileName { get; set; } = null!;
	}
}
