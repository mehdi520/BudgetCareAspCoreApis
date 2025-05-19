using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class ScheduleBond
{
    public int Id { get; set; }

    public int YearId { get; set; }

    public string Amount { get; set; } = null!;

    public bool IsPremium { get; set; }

    public string Day { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Place { get; set; } = null!;

    public bool IsAnnounced { get; set; }

    public virtual ICollection<BondsDraw> BondsDraws { get; set; } = new List<BondsDraw>();

    public virtual BondsRecordsYear Year { get; set; } = null!;
}
