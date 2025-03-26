using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class GroupDetail
{
    public string GroupNo { get; set; } = null!;

    public string EmployeeID { get; set; } = null!;

    public string Teamleader { get; set; } = null!;

    public string? GroupContent { get; set; }

    public virtual Employee? Employee { get; set; } 

    public virtual Group? GroupNoNavigation { get; set; } 

    public virtual Employee? TeamleaderNavigation { get; set; } 
}
