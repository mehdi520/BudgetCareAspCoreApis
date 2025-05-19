using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class BondsRecordsYear
{
    public int Id { get; set; }

    public int? Year { get; set; }

    public virtual ICollection<ScheduleBond> ScheduleBonds { get; set; } = new List<ScheduleBond>();
}
