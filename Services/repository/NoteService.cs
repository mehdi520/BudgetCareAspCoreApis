using BudgetCareApis.Data;
using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ReqModels.Notes;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.Notes;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace BudgetCareApis.Services.repository
{
	/// <summary>
	/// Notebooks, notes and note attachments.
	/// Attachments are stored on disk under {UploadsRoot}/uploads/notes/{noteId}/{fileName}
	/// and the relative path ("uploads/notes/{noteId}/{fileName}") is saved in the database.
	/// </summary>
	public class NoteService : INoteService
	{
		public const string UploadsFolder = "uploads";
		public const string NotesFolder = "notes";

		private const int DefaultPageSize = 10;
		private const int MaxPageSize = 100;

		private static readonly HashSet<string> BlockedExtensions = new(StringComparer.OrdinalIgnoreCase)
		{
			".exe", ".dll", ".bat", ".cmd", ".com", ".msi", ".ps1", ".vbs", ".scr", ".sh", ".jar", ".cpl", ".msc"
		};

		private static readonly FileExtensionContentTypeProvider ContentTypes = new();

		private readonly BudgetCareDBContext _context;
		private readonly string _uploadsRoot;
		private readonly long _maxFileSize;
		private readonly ILogger<NoteService> _logger;

		public NoteService(BudgetCareDBContext context, IWebHostEnvironment env, IConfiguration config, ILogger<NoteService> logger)
		{
			_context = context;
			_logger = logger;
			// The folder that contains "uploads". Defaults to the app's content root.
			var root = config.GetValue<string>("Uploads:RootPath");
			_uploadsRoot = string.IsNullOrWhiteSpace(root) ? env.ContentRootPath : root;
			_maxFileSize = config.GetValue<long?>("Uploads:MaxFileSizeMB").GetValueOrDefault(25) * 1024 * 1024;
		}

		#region Notebooks

		public async Task<GetUserNoteBookResModel> GetNoteBooks(GetNoteBooksReqModel req, int userId)
		{
			var res = new GetUserNoteBookResModel();
			try
			{
				var query = _context.NoteBooks.AsNoTracking().Where(x => x.UserId == userId);
				if (!string.IsNullOrWhiteSpace(req.search))
				{
					var term = req.search.Trim();
					query = query.Where(x => x.Title.Contains(term));
				}

				var total = await query.CountAsync();

				// pageNo <= 0 => return everything (keeps old clients working).
				var projected = query
					.OrderBy(x => x.Title)
					.Select(x => new NoteBookModel
					{
						NoteBookId = x.NoteBookId,
						UserId = x.UserId,
						Title = x.Title,
						IconColor = x.IconColor,
						TotalNotes = x.Notes.Count()
					});

				if (req.pageNo > 0)
				{
					var size = NormalizePageSize(req.pageSize);
					var page = ClampPage(req.pageNo, size, total);
					res.data = await projected.Skip((page - 1) * size).Take(size).ToListAsync();
					res.SetPaging(page, size, total);
				}
				else
				{
					res.data = await projected.ToListAsync();
					res.SetPaging(1, total, total);
				}

				res.Status = true;
				res.Message = "fetch successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<SaveResponseModel> AddUpdateNoteBook(NoteBookModel req)
		{
			var res = new SaveResponseModel();
			try
			{
				var title = req.Title?.Trim();
				if (string.IsNullOrEmpty(title))
				{
					res.Message = "Title is required";
					return res;
				}

				var duplicate = await _context.NoteBooks.AnyAsync(x =>
					x.UserId == req.UserId && x.Title == title && x.NoteBookId != req.NoteBookId);
				if (duplicate)
				{
					res.Message = "A notebook with the same title already exists. Please use a different title";
					return res;
				}

				if (req.NoteBookId == 0)
				{
					var book = new NoteBook { UserId = req.UserId, Title = title, IconColor = req.IconColor ?? "#9990FF" };
					_context.NoteBooks.Add(book);
					await _context.SaveChangesAsync();
					res.Id = book.NoteBookId;
					res.Status = true;
					res.Message = "Notebook saved successfully";
				}
				else
				{
					var book = await _context.NoteBooks.FirstOrDefaultAsync(x => x.NoteBookId == req.NoteBookId && x.UserId == req.UserId);
					if (book == null)
					{
						res.Message = "Notebook not found";
						return res;
					}
					book.Title = title;
					book.IconColor = req.IconColor ?? book.IconColor;
					await _context.SaveChangesAsync();
					res.Id = book.NoteBookId;
					res.Status = true;
					res.Message = "Notebook updated successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<BaseResponseModel> DeleteNoteBook(int noteBookId, int userId)
		{
			var res = new BaseResponseModel();
			try
			{
				var book = await _context.NoteBooks.FirstOrDefaultAsync(x => x.NoteBookId == noteBookId && x.UserId == userId);
				if (book == null)
				{
					res.Message = "Notebook not found";
					return res;
				}

				var notes = await _context.Notes.Include(x => x.Attachments).Where(x => x.NoteBookId == noteBookId).ToListAsync();
				var noteIds = notes.Select(x => x.NoteId).ToList();

				_context.NoteAttachments.RemoveRange(notes.SelectMany(x => x.Attachments));
				_context.Notes.RemoveRange(notes);
				_context.NoteBooks.Remove(book);
				await _context.SaveChangesAsync();

				foreach (var id in noteIds) DeleteNoteFolder(id);

				res.Status = true;
				res.Message = "Notebook deleted successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<GetNoteBookExportResModel> GetNoteBookExport(int noteBookId, int userId)
		{
			var res = new GetNoteBookExportResModel();
			try
			{
				var book = await _context.NoteBooks.AsNoTracking()
					.Where(x => x.NoteBookId == noteBookId && x.UserId == userId)
					.Select(x => new NoteBookModel
					{
						NoteBookId = x.NoteBookId,
						UserId = x.UserId,
						Title = x.Title,
						IconColor = x.IconColor,
						TotalNotes = x.Notes.Count()
					})
					.FirstOrDefaultAsync();

				if (book == null)
				{
					res.Message = "Notebook not found";
					return res;
				}

				var notes = await _context.Notes.AsNoTracking()
					.Where(x => x.NoteBookId == noteBookId && x.UserId == userId)
					.Include(x => x.Attachments)
					.OrderBy(x => x.CreatedAt).ThenBy(x => x.NoteId)
					.ToListAsync();

				res.noteBook = book;
				res.data = notes.Select(n => ToModel(n, book.Title, includeAttachments: true)).ToList();
				res.Status = true;
				res.Message = "fetch successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		#endregion

		#region Notes

		public async Task<GetNotesResModel> GetNotes(GetNotesReqModel req, int userId)
		{
			var res = new GetNotesResModel();
			try
			{
				var size = NormalizePageSize(req.pageSize);

				var query = _context.Notes.AsNoTracking().Where(x => x.UserId == userId);
				if (req.categoryId > 0)
					query = query.Where(x => x.NoteBookId == req.categoryId);

				if (!string.IsNullOrWhiteSpace(req.search))
				{
					var term = req.search.Trim();
					query = query.Where(x => x.Title.Contains(term) || x.Details.Contains(term));
				}

				var total = await query.CountAsync();
				var page = ClampPage(req.pageNo, size, total);

				res.data = await query
					.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.NoteId)
					.Skip((page - 1) * size)
					.Take(size)
					.Select(x => new NoteDataModel
					{
						NoteId = x.NoteId,
						NoteBookId = x.NoteBookId,
						UserId = x.UserId,
						CreatedAt = x.CreatedAt,
						Title = x.Title,
						Details = x.Details,
						NoteBookName = x.NoteBook.Title,
						AttachmentCount = x.Attachments.Count()
					})
					.ToListAsync();

				res.SetPaging(page, size, total);
				res.Status = true;
				res.Message = "fetch successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<GetNoteResModel> GetNote(int noteId, int userId)
		{
			var res = new GetNoteResModel();
			try
			{
				var note = await _context.Notes.AsNoTracking()
					.Include(x => x.NoteBook)
					.Include(x => x.Attachments)
					.FirstOrDefaultAsync(x => x.NoteId == noteId && x.UserId == userId);

				if (note == null)
				{
					res.Message = "Note not found";
					return res;
				}

				res.data = ToModel(note, note.NoteBook.Title, includeAttachments: true);
				res.Status = true;
				res.Message = "fetch successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<SaveResponseModel> AddUpdateNote(NoteDataModel req)
		{
			var res = new SaveResponseModel();
			try
			{
				var title = req.Title?.Trim();
				if (string.IsNullOrEmpty(title))
				{
					res.Message = "Title is required";
					return res;
				}

				var bookOwned = await _context.NoteBooks.AnyAsync(x => x.NoteBookId == req.NoteBookId && x.UserId == req.UserId);
				if (!bookOwned)
				{
					res.Message = "Notebook not found";
					return res;
				}

				var duplicate = await _context.Notes.AnyAsync(x =>
					x.UserId == req.UserId && x.NoteBookId == req.NoteBookId && x.Title == title && x.NoteId != req.NoteId);
				if (duplicate)
				{
					res.Message = "A note with the same title already exists in this notebook. Please use a different title";
					return res;
				}

				if (req.NoteId == 0)
				{
					var note = new Note
					{
						Title = title,
						Details = req.Details ?? string.Empty,
						NoteBookId = req.NoteBookId,
						UserId = req.UserId,
						CreatedAt = DateTime.Now
					};
					_context.Notes.Add(note);
					await _context.SaveChangesAsync();
					res.Id = note.NoteId;
					res.Status = true;
					res.Message = "Note saved successfully";
				}
				else
				{
					var note = await _context.Notes.FirstOrDefaultAsync(x => x.NoteId == req.NoteId && x.UserId == req.UserId);
					if (note == null)
					{
						res.Message = "Note not found";
						return res;
					}
					note.Title = title;
					note.Details = req.Details ?? string.Empty;
					note.NoteBookId = req.NoteBookId;
					await _context.SaveChangesAsync();
					res.Id = note.NoteId;
					res.Status = true;
					res.Message = "Note updated successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<BaseResponseModel> DeleteNote(int noteId, int userId)
		{
			var res = new BaseResponseModel();
			try
			{
				var note = await _context.Notes.Include(x => x.Attachments)
					.FirstOrDefaultAsync(x => x.NoteId == noteId && x.UserId == userId);
				if (note == null)
				{
					res.Message = "Note not found";
					return res;
				}

				_context.NoteAttachments.RemoveRange(note.Attachments);
				_context.Notes.Remove(note);
				await _context.SaveChangesAsync();
				DeleteNoteFolder(noteId);

				res.Status = true;
				res.Message = "Note deleted successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		#endregion

		#region Attachments

		public async Task<GetAttachmentsResModel> GetAttachments(int noteId, int userId)
		{
			var res = new GetAttachmentsResModel();
			try
			{
				var owned = await _context.Notes.AnyAsync(x => x.NoteId == noteId && x.UserId == userId);
				if (!owned)
				{
					res.Message = "Note not found";
					return res;
				}

				var list = await _context.NoteAttachments.AsNoTracking()
					.Where(x => x.NoteId == noteId)
					.OrderBy(x => x.CreatedAt).ThenBy(x => x.AttachmentId)
					.ToListAsync();

				res.data = list.Select(ToModel).ToList();
				res.Status = true;
				res.Message = "fetch successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<GetAttachmentsResModel> UploadAttachments(int noteId, int userId, IReadOnlyList<IFormFile> files)
		{
			var res = new GetAttachmentsResModel();
			var savedPaths = new List<string>();
			try
			{
				var owned = await _context.Notes.AnyAsync(x => x.NoteId == noteId && x.UserId == userId);
				if (!owned)
				{
					res.Message = "Note not found";
					return res;
				}

				if (files == null || files.Count == 0)
				{
					res.Message = "Please select at least one file";
					return res;
				}

				foreach (var file in files)
				{
					var error = ValidateFile(file);
					if (error != null)
					{
						res.Message = error;
						return res;
					}
				}

				var added = new List<NoteAttachment>();
				foreach (var file in files)
				{
					var (relativePath, fileName) = await SaveFileAsync(noteId, file, file.FileName);
					savedPaths.Add(relativePath);
					var entity = new NoteAttachment
					{
						NoteId = noteId,
						UserId = userId,
						FileName = fileName,
						FilePath = relativePath,
						ContentType = ResolveContentType(file, fileName),
						FileSize = file.Length,
						CreatedAt = DateTime.Now
					};
					_context.NoteAttachments.Add(entity);
					added.Add(entity);
				}
				await _context.SaveChangesAsync();

				res.data = added.Select(ToModel).ToList();
				res.Status = true;
				res.Message = added.Count == 1 ? "Attachment uploaded successfully" : $"{added.Count} attachments uploaded successfully";
			}
			catch (Exception ex)
			{
				// Don't leave orphan files behind if the DB save failed.
				foreach (var p in savedPaths) TryDeleteFile(p);
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<GetAttachmentsResModel> ReplaceAttachment(int attachmentId, int userId, IFormFile file)
		{
			var res = new GetAttachmentsResModel();
			try
			{
				var entity = await _context.NoteAttachments.FirstOrDefaultAsync(x => x.AttachmentId == attachmentId && x.UserId == userId);
				if (entity == null)
				{
					res.Message = "Attachment not found";
					return res;
				}

				var error = ValidateFile(file);
				if (error != null)
				{
					res.Message = error;
					return res;
				}

				var oldPath = entity.FilePath;
				var (relativePath, fileName) = await SaveFileAsync(entity.NoteId, file, file.FileName);

				entity.FileName = fileName;
				entity.FilePath = relativePath;
				entity.ContentType = ResolveContentType(file, fileName);
				entity.FileSize = file.Length;
				entity.UpdatedAt = DateTime.Now;
				await _context.SaveChangesAsync();

				if (!string.Equals(oldPath, relativePath, StringComparison.OrdinalIgnoreCase))
					TryDeleteFile(oldPath);

				res.data = new List<NoteAttachmentModel> { ToModel(entity) };
				res.Status = true;
				res.Message = "Attachment replaced successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<BaseResponseModel> RenameAttachment(RenameAttachmentReqModel req, int userId)
		{
			var res = new BaseResponseModel();
			try
			{
				var entity = await _context.NoteAttachments.FirstOrDefaultAsync(x => x.AttachmentId == req.attachmentId && x.UserId == userId);
				if (entity == null)
				{
					res.Message = "Attachment not found";
					return res;
				}

				var newName = SanitizeFileName(req.fileName);
				if (string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(newName)))
				{
					res.Message = "File name is required";
					return res;
				}

				// Keep the original extension if the user removed it.
				if (string.IsNullOrEmpty(Path.GetExtension(newName)))
					newName += Path.GetExtension(entity.FileName);

				if (BlockedExtensions.Contains(Path.GetExtension(newName)))
				{
					res.Message = "This file type is not allowed";
					return res;
				}

				if (newName != entity.FileName)
				{
					var oldPhysical = ToPhysicalPath(entity.FilePath);
					var folder = GetNoteFolder(entity.NoteId);
					var uniqueName = MakeUniqueName(folder, newName);
					var newPhysical = Path.Combine(folder, uniqueName);
					if (File.Exists(oldPhysical))
						File.Move(oldPhysical, newPhysical);

					entity.FileName = uniqueName;
					entity.FilePath = ToRelativePath(entity.NoteId, uniqueName);
					entity.UpdatedAt = DateTime.Now;
					await _context.SaveChangesAsync();
				}

				res.Status = true;
				res.Message = "Attachment renamed successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<BaseResponseModel> DeleteAttachment(int attachmentId, int userId)
		{
			var res = new BaseResponseModel();
			try
			{
				var entity = await _context.NoteAttachments.FirstOrDefaultAsync(x => x.AttachmentId == attachmentId && x.UserId == userId);
				if (entity == null)
				{
					res.Message = "Attachment not found";
					return res;
				}

				_context.NoteAttachments.Remove(entity);
				await _context.SaveChangesAsync();
				TryDeleteFile(entity.FilePath);

				res.Status = true;
				res.Message = "Attachment deleted successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<AttachmentFileResult?> GetAttachmentFile(int attachmentId, int userId)
		{
			var entity = await _context.NoteAttachments.AsNoTracking()
				.FirstOrDefaultAsync(x => x.AttachmentId == attachmentId && x.UserId == userId);
			if (entity == null) return null;

			var physical = ToPhysicalPath(entity.FilePath);
			if (!File.Exists(physical)) return null;

			return new AttachmentFileResult(physical, entity.ContentType, entity.FileName);
		}

		#endregion

		#region Helpers

		private static int NormalizePageSize(int pageSize)
			=> pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

		/// <summary>Keeps the page inside 1..totalPages so deleting the last item of a page doesn't return an empty page.</summary>
		private static int ClampPage(int pageNo, int pageSize, int total)
		{
			var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
			if (pageNo < 1) return 1;
			return Math.Min(pageNo, totalPages);
		}

		private static NoteDataModel ToModel(Note n, string? noteBookName, bool includeAttachments)
		{
			var attachments = n.Attachments
				.OrderBy(a => a.CreatedAt).ThenBy(a => a.AttachmentId)
				.Select(ToModel).ToList();

			return new NoteDataModel
			{
				NoteId = n.NoteId,
				NoteBookId = n.NoteBookId,
				UserId = n.UserId,
				CreatedAt = n.CreatedAt,
				Title = n.Title,
				Details = n.Details,
				NoteBookName = noteBookName,
				AttachmentCount = attachments.Count,
				Attachments = includeAttachments ? attachments : null
			};
		}

		private static NoteAttachmentModel ToModel(NoteAttachment a) => new()
		{
			AttachmentId = a.AttachmentId,
			NoteId = a.NoteId,
			FileName = a.FileName,
			FilePath = a.FilePath,
			ContentType = a.ContentType,
			FileSize = a.FileSize,
			IsImage = a.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase),
			CreatedAt = a.CreatedAt
		};

		private string? ValidateFile(IFormFile? file)
		{
			if (file == null || file.Length == 0)
				return "Empty file is not allowed";
			if (file.Length > _maxFileSize)
				return $"'{file.FileName}' is too large. Maximum size is {_maxFileSize / (1024 * 1024)} MB";
			if (BlockedExtensions.Contains(Path.GetExtension(file.FileName)))
				return $"'{file.FileName}': this file type is not allowed";
			return null;
		}

		private static string ResolveContentType(IFormFile file, string fileName)
		{
			if (ContentTypes.TryGetContentType(fileName, out var type))
				return type;
			return string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
		}

		private string GetNoteFolder(int noteId)
			=> Path.Combine(_uploadsRoot, UploadsFolder, NotesFolder, noteId.ToString());

		private static string ToRelativePath(int noteId, string fileName)
			=> $"{UploadsFolder}/{NotesFolder}/{noteId}/{fileName}";

		private string ToPhysicalPath(string relativePath)
		{
			var full = Path.GetFullPath(Path.Combine(_uploadsRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));
			var allowedRoot = Path.GetFullPath(Path.Combine(_uploadsRoot, UploadsFolder)) + Path.DirectorySeparatorChar;
			if (!full.StartsWith(allowedRoot, StringComparison.OrdinalIgnoreCase))
				throw new InvalidOperationException("Invalid attachment path");
			return full;
		}

		private async Task<(string relativePath, string fileName)> SaveFileAsync(int noteId, IFormFile file, string requestedName)
		{
			var folder = GetNoteFolder(noteId);
			Directory.CreateDirectory(folder);

			var fileName = MakeUniqueName(folder, SanitizeFileName(requestedName));
			var physical = Path.Combine(folder, fileName);

			await using (var stream = new FileStream(physical, FileMode.CreateNew))
			{
				await file.CopyToAsync(stream);
			}
			return (ToRelativePath(noteId, fileName), fileName);
		}

		private static string SanitizeFileName(string? name)
		{
			name = Path.GetFileName(name ?? string.Empty).Trim();
			var invalid = Path.GetInvalidFileNameChars();
			var cleaned = new string(name.Select(c => invalid.Contains(c) || c == '/' || c == '\\' ? '_' : c).ToArray());
			cleaned = cleaned.Trim('.', ' ');

			var ext = Path.GetExtension(cleaned);
			var baseName = Path.GetFileNameWithoutExtension(cleaned);
			if (baseName.Length > 120) baseName = baseName[..120];
			if (string.IsNullOrEmpty(baseName)) baseName = "file";
			return baseName + ext;
		}

		/// <summary>abc.png, abc_1.png, abc_2.png ...</summary>
		private static string MakeUniqueName(string folder, string fileName)
		{
			var baseName = Path.GetFileNameWithoutExtension(fileName);
			var ext = Path.GetExtension(fileName);
			var candidate = fileName;
			var i = 1;
			while (File.Exists(Path.Combine(folder, candidate)))
				candidate = $"{baseName}_{i++}{ext}";
			return candidate;
		}

		private void TryDeleteFile(string relativePath)
		{
			try
			{
				var physical = ToPhysicalPath(relativePath);
				if (File.Exists(physical)) File.Delete(physical);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Could not delete attachment file {Path}", relativePath);
			}
		}

		private void DeleteNoteFolder(int noteId)
		{
			try
			{
				var folder = GetNoteFolder(noteId);
				if (Directory.Exists(folder)) Directory.Delete(folder, recursive: true);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Could not delete attachment folder for note {NoteId}", noteId);
			}
		}

		#endregion
	}
}
