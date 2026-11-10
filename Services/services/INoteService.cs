using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ReqModels.Notes;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.Notes;

namespace BudgetCareApis.Services.services
{
	public interface INoteService
	{
		// Notebooks (categories)
		Task<GetUserNoteBookResModel> GetNoteBooks(GetNoteBooksReqModel req, int userId);
		Task<SaveResponseModel> AddUpdateNoteBook(NoteBookModel req);
		Task<BaseResponseModel> DeleteNoteBook(int noteBookId, int userId);
		Task<GetNoteBookExportResModel> GetNoteBookExport(int noteBookId, int userId);

		// Notes
		Task<GetNotesResModel> GetNotes(GetNotesReqModel req, int userId);
		Task<GetNoteResModel> GetNote(int noteId, int userId);
		Task<SaveResponseModel> AddUpdateNote(NoteDataModel req);
		Task<BaseResponseModel> DeleteNote(int noteId, int userId);

		// Attachments
		Task<GetAttachmentsResModel> GetAttachments(int noteId, int userId);
		Task<GetAttachmentsResModel> UploadAttachments(int noteId, int userId, IReadOnlyList<IFormFile> files);
		Task<GetAttachmentsResModel> ReplaceAttachment(int attachmentId, int userId, IFormFile file);
		Task<BaseResponseModel> RenameAttachment(RenameAttachmentReqModel req, int userId);
		Task<BaseResponseModel> DeleteAttachment(int attachmentId, int userId);
		Task<AttachmentFileResult?> GetAttachmentFile(int attachmentId, int userId);
	}

	public record AttachmentFileResult(string PhysicalPath, string ContentType, string FileName);
}
