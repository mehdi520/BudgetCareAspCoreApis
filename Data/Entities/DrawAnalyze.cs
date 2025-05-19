using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class DrawAnalyze
{
    public int Id { get; set; }

    public string Json { get; set; } = null!;
}
