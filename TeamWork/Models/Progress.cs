using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Progress
{
    public int ProgressNo { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Time { get; set; }

    public string TaskID { get; set; } = null!;

    public string? EmployeeID { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Task Task { get; set; } = null!;
}
