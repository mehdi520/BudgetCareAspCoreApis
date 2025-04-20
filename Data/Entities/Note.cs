using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class Note
{
    public int NoteId { get; set; }

    public int NoteBookId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string Details { get; set; } = null!;

    public virtual NoteBook NoteBook { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
