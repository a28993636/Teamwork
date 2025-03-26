using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class TaskStatus
{
    public string StatusNo { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Task> Task { get; set; } = new List<Task>();
}
