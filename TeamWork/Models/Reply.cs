using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Reply
{
    public string ReplyNo { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Time { get; set; }

    public string? MessageNo { get; set; }

    public string? EmployeeID { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Message? MessageNoNavigation { get; set; }
}
