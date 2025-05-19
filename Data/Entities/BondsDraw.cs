using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class BondsDraw
{
    public int DrawId { get; set; }

    public int ScheduleId { get; set; }

    public DateTime DrawDate { get; set; }

    public int DrawNo { get; set; }

    public string FirstPrizeWorth { get; set; } = null!;

    public string SecondPrizeWorth { get; set; } = null!;

    public string ThirdPrizeWorth { get; set; } = null!;

    public virtual ICollection<DrawWinsBond> DrawWinsBonds { get; set; } = new List<DrawWinsBond>();

    public virtual ScheduleBond Schedule { get; set; } = null!;
}
