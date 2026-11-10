using System;

namespace BudgetCareApis.Data.Entities;

public partial class NoteAttachment
{
    public int AttachmentId { get; set; }

    public int NoteId { get; set; }

    public int UserId { get; set; }

    /// <summary>Original (display) file name, e.g. "abc.png".</summary>
    public string FileName { get; set; } = null!;

    /// <summary>Relative path on disk, e.g. "uploads/notes/12/abc.png".</summary>
    public string FilePath { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Note Note { get; set; } = null!;
}
