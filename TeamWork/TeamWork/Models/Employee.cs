using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Employee
{
    public string EmployeeID { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Tel { get; set; } = null!;

    public string PositionNo { get; set; } = null!;

    public string AccountID { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<GroupDetail> GroupDetailEmployee { get; set; } = new List<GroupDetail>();

    public virtual ICollection<GroupDetail> GroupDetailTeamleaderNavigation { get; set; } = new List<GroupDetail>();

    public virtual ICollection<Message> Message { get; set; } = new List<Message>();

    public virtual Position PositionNoNavigation { get; set; } = null!;

    public virtual ICollection<Progress> Progress { get; set; } = new List<Progress>();

    public virtual ICollection<Reply> Reply { get; set; } = new List<Reply>();

    public virtual ICollection<Schedule> Schedule { get; set; } = new List<Schedule>();

    public virtual ICollection<TaskDetail> TaskDetail { get; set; } = new List<TaskDetail>();
}
