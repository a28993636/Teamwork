using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Message
{
    public string MessageNo { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Time { get; set; }

    public string TaskID { get; set; } = null!;

    public string? EmployeeID { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Reply> Reply { get; set; } = new List<Reply>();

    public virtual Task Task { get; set; } = null!;
}
