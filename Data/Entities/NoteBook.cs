using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class NoteBook
{
    public int NoteBookId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string IconColor { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual User User { get; set; } = null!;
}
