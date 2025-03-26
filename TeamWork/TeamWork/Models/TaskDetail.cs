using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class TaskDetail
{
    public string TaskID { get; set; } = null!;

    public string EmployeeID { get; set; } = null!;

    public string TaskContent { get; set; } = null!;

    public virtual Employee? Employee { get; set; } = null!;

    public virtual Task? Task { get; set; } = null!;
}
