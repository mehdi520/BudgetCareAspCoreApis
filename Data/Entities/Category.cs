using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public int? UserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public virtual User? User { get; set; }
}
