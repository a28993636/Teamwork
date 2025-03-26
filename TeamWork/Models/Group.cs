using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Group
{
    public string GroupNo { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<GroupDetail> GroupDetail { get; set; } = new List<GroupDetail>();

    public virtual ICollection<Task> Task { get; set; } = new List<Task>();
}
