using System;
using System.Collections.Generic;

namespace Teamwork.Models;

public partial class Account
{
    public string AccountID { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();
}
