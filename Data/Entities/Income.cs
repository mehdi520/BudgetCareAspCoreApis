using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class Income
{
    public int Id { get; set; }

    public int CatId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string? Desciption { get; set; }

    public DateOnly Date { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
