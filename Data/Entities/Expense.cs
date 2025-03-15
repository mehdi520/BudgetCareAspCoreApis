using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class Expense
{
    public int Id { get; set; }

    public int CatId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset Date { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? Description { get; set; }
}
