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

    public DateTimeOffset Date { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
