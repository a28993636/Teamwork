using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class TaskType
{
    public string TaskTypeNo { get; set; } = null!;

    public string TaskTypeName { get; set; } = null!;

    public virtual ICollection<Task> Task { get; set; } = new List<Task>();
}
