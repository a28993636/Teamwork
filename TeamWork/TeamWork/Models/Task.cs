using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Task
{
    public string TaskID { get; set; } = null!;

    public string TaskName { get; set; } = null!;

    public DateTime TaskCreateDate { get; set; }

    public DateTime TaskStartDate { get; set; }

    public DateTime TaskEndDate { get; set; }

    public string StatusNo { get; set; } = null!;

    public string TaskTypeNo { get; set; } = null!;

    public string? GroupNo { get; set; }

    public virtual Group? GroupNoNavigation { get; set; }

    public virtual ICollection<Message> Message { get; set; } = new List<Message>();

    public virtual ICollection<Progress> Progress { get; set; } = new List<Progress>();

    public virtual ICollection<Schedule> Schedule { get; set; } = new List<Schedule>();

    public virtual TaskStatus? StatusNoNavigation { get; set; } = null!;

    public virtual ICollection<TaskDetail> TaskDetail { get; set; } = new List<TaskDetail>();

    public virtual TaskType? TaskTypeNoNavigation { get; set; } = null!;
}
