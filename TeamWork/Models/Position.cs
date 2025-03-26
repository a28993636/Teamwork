using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Position
{
    public string PositionNo { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();
}
