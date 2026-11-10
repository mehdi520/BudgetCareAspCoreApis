using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ReqModels.Notes;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetCareApis.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class NoteController : BaseController
	{
		// Max size of one upload request (all files together).
		private const long MaxUploadRequestBytes = 200L * 1024 * 1024;

		private readonly INoteService _noteService;
		private readonly ILogger<NoteController> _logger;

		public NoteController(ILogger<NoteController> logger, INoteService noteService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_noteService = noteService;
		}

		// ---------------- Notes ----------------

		/// <summary>
		/// Paged notes. categoryId = notebook id (0 = all notebooks), search matches title/details.
		/// Response includes currentPage, pageSize, totalPage, totalCount.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> GetUserNotes(GetNotesReqModel req)
		{
			var res = await _noteService.GetNotes(req, getLoggedinUserId());
			return Ok(res);
		}

		/// <summary>Single note with all attachments.</summary>
		[HttpGet]
		public async Task<IActionResult> GetNote(int noteId)
		{
			var res = await _noteService.GetNote(noteId, getLoggedinUserId());
			return Ok(res);
		}

		/// <summary>All notes of a notebook with attachments (used for the notebook PDF).</summary>
		[HttpGet]
		public async Task<IActionResult> GetNoteBookExport(int noteBookId)
		{
			var res = await _noteService.GetNoteBookExport(noteBookId, getLoggedinUserId());
			return Ok(res);
		}

		/// <summary>Create (noteId = 0) or update a note. Response "id" is the saved note id.</summary>
		[HttpPost]
		public async Task<IActionResult> addOrUpdateNote(NoteDataModel req)
		{
			if (string.IsNullOrWhiteSpace(req.Title))
				return Ok(new BaseResponseModel { Status = false, Message = "Title is required" });
			if (req.NoteBookId == 0)
				return Ok(new BaseResponseModel { Status = false, Message = "Notebook is required" });

			req.UserId = getLoggedinUserId();
			var res = await _noteService.AddUpdateNote(req);
			return Ok(res);
		}

		/// <summary>Deletes the note, its attachments and its uploads/notes/{noteId} folder.</summary>
		[HttpGet]
		public async Task<IActionResult> delNote(int noteId)
		{
			var res = await _noteService.DeleteNote(noteId, getLoggedinUserId());
			return Ok(res);
		}

		// ---------------- Attachments ----------------

		[HttpGet]
		public async Task<IActionResult> GetAttachments(int noteId)
		{
			var res = await _noteService.GetAttachments(noteId, getLoggedinUserId());
			return Ok(res);
		}

		/// <summary>
		/// multipart/form-data: noteId + one or more "files".
		/// Files are saved to uploads/notes/{noteId}/{fileName}.
		/// </summary>
		[HttpPost]
		[Consumes("multipart/form-data")]
		[RequestSizeLimit(MaxUploadRequestBytes)]
		[RequestFormLimits(MultipartBodyLengthLimit = MaxUploadRequestBytes)]
		public async Task<IActionResult> UploadAttachments([FromForm] int noteId, [FromForm] List<IFormFile> files)
		{
			var res = await _noteService.UploadAttachments(noteId, getLoggedinUserId(), files);
			return Ok(res);
		}

		/// <summary>multipart/form-data: attachmentId + "file". Replaces the stored file.</summary>
		[HttpPost]
		[Consumes("multipart/form-data")]
		[RequestSizeLimit(MaxUploadRequestBytes)]
		[RequestFormLimits(MultipartBodyLengthLimit = MaxUploadRequestBytes)]
		public async Task<IActionResult> ReplaceAttachment([FromForm] int attachmentId, IFormFile file)
		{
			var res = await _noteService.ReplaceAttachment(attachmentId, getLoggedinUserId(), file);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> RenameAttachment(RenameAttachmentReqModel req)
		{
			var res = await _noteService.RenameAttachment(req, getLoggedinUserId());
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteAttachment(int attachmentId)
		{
			var res = await _noteService.DeleteAttachment(attachmentId, getLoggedinUserId());
			return Ok(res);
		}

		/// <summary>Returns the file bytes. Only the owner can download (files are not public).</summary>
		[HttpGet]
		public async Task<IActionResult> DownloadAttachment(int attachmentId)
		{
			var file = await _noteService.GetAttachmentFile(attachmentId, getLoggedinUserId());
			if (file == null)
				return NotFound(new BaseResponseModel { Status = false, Message = "Attachment not found" });

			return PhysicalFile(file.PhysicalPath, file.ContentType, file.FileName);
		}
	}
}
