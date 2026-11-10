namespace BudgetCareApis.Models.Dtos.Notes
{
	public class NoteAttachmentModel
	{
		public int AttachmentId { get; set; }
		public int NoteId { get; set; }
		public string FileName { get; set; } = null!;
		/// <summary>Relative path, e.g. "uploads/notes/12/abc.png". Download through Note/DownloadAttachment.</summary>
		public string FilePath { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public long FileSize { get; set; }
		public bool IsImage { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
