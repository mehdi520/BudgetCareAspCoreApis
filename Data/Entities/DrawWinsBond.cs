using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class DrawWinsBond
{
    public int Id { get; set; }

    public int DrawId { get; set; }

    public string BoundNo { get; set; } = null!;

    public int Position { get; set; }

    public virtual BondsDraw Draw { get; set; } = null!;
}
