using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<NoteBook> NoteBooks { get; set; } = new List<NoteBook>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
