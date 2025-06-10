using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class BondType
{
    public int TypeId { get; set; }

    public string BondType1 { get; set; } = null!;

    public bool IsPermium { get; set; }

    public virtual ICollection<BondsDraw> BondsDraws { get; set; } = new List<BondsDraw>();

    public virtual ICollection<UserBond> UserBonds { get; set; } = new List<UserBond>();
}
