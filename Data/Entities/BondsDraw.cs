using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class BondsDraw
{
    public int DrawId { get; set; }

    public int BondTypeId { get; set; }

    public DateTime DrawDate { get; set; }

    public int? DrawNo { get; set; }

    public string? FirstPrizeWorth { get; set; }

    public string? SecondPrizeWorth { get; set; }

    public string? ThirdPrizeWorth { get; set; }

    public bool IsResultAnnounced { get; set; }

    public string? Place { get; set; }

    public string? Day { get; set; }

    public virtual BondType BondType { get; set; } = null!;

    public virtual ICollection<DrawWinsBond> DrawWinsBonds { get; set; } = new List<DrawWinsBond>();

    public virtual ICollection<UserWonBond> UserWonBonds { get; set; } = new List<UserWonBond>();
}
