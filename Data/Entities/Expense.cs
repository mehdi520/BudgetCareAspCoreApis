using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class Expense
{
    public int Id { get; set; }

    public int CatId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Description { get; set; }
}
