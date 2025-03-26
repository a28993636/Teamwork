using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Schedule
{
    public string ScheduleNo { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? TaskID { get; set; }

    public string EmployeeID { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Task? Task { get; set; }
}
