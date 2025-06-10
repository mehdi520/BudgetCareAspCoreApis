using System;
using System.Collections.Generic;

namespace BudgetCareApis.Data.Entities;

public partial class UserBond
{
    public int BondId { get; set; }

    public string BondNumber { get; set; } = null!;

    public int BondType { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int UserId { get; set; }

    public virtual BondType BondTypeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserWonBond> UserWonBonds { get; set; } = new List<UserWonBond>();
}
