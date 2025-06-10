using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class UserWonBond
{
    public int WonId { get; set; }

    public string Status { get; set; } = null!;

    public int BondId { get; set; }

    public int DrawId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int Position { get; set; }

    public virtual UserBond Bond { get; set; } = null!;

    public virtual BondsDraw Draw { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
