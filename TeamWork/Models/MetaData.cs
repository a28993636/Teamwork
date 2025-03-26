using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teamwork.Models;

public class AccountData
{
    [Key]
    [Display(Name = "帳號")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "員工編號")]
    [Required(ErrorMessage = "必填")]
    public string AccountID { get; set; } = null!;

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "必填")]
    [StringLength(15, MinimumLength = 8, ErrorMessage = "密碼為8-15碼")]
    [MinLength(8)]
    [MaxLength(16)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

   
}
[ModelMetadataType(typeof(AccountData))]
public partial class Account
{
}


public class EmployeeData
{
    [Display(Name = "員工編號")]
    [StringLength(6, MinimumLength = 6)]
    [Key]
    public string EmployeeID { get; set; } = null!;

    [Display(Name = "姓名")]
    [StringLength(24, MinimumLength = 2)]
    [Required(ErrorMessage = "必填")]
    public string EmployeeName { get; set; } = null!;


    [Display(Name = "入職日")]
    [Required(ErrorMessage = "必填")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "離職日")]
    [Required(ErrorMessage = "必填")]
    public DateOnly? EndDate { get; set; }

    [Display(Name = "手機號碼")]
    [Required(ErrorMessage = "必填")]
    public string Tel { get; set; } = null!;

    
}
[ModelMetadataType(typeof(EmployeeData))]
public partial class Employee
{
}


public class GroupData
{
    [Display(Name = "小組編號")]
    [StringLength(7, MinimumLength = 7)]
    [Key]
    public string GroupNo { get; set; } = null!;

    [Display(Name = "小組名稱")]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "名稱最多25個字")]
    [Required(ErrorMessage = "必填")]
    public string GroupName { get; set; } = null!;

    [Display(Name = "開始日期")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "必填")]
    public DateTime StartDate { get; set; }

    [Display(Name = "結束日期")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "必填")]
    public DateTime EndDate { get; set; }

  

}
[ModelMetadataType(typeof(GroupData))]
public partial class Group
{
}


public class GroupDetailData
{
    public string GroupNo { get; set; } = null!;

    [Display(Name = "小組成員")]
    [Required(ErrorMessage = "必填")]
    public string EmployeeID { get; set; } = null!;

    public string Teamleader { get; set; } = null!;

    [Display(Name = "小組簡介")]
    [StringLength(70, MinimumLength = 1, ErrorMessage = "最多70個字")]
    public string? GroupContent { get; set; }

    public virtual Employee Employee { get; set; } = null!;


    public virtual Group GroupNoNavigation { get; set; } = null!;
}
[ModelMetadataType(typeof(GroupDetailData))]
public partial class GroupDetail
{
}


public class MessageData
{
    [Display(Name = "留言編號")]
    [StringLength(3, MinimumLength = 3)]
    [Key]
    public string MessageNo { get; set; } = null!;

    [Display(Name = "主旨")]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "最多25個字")]
    [Required(ErrorMessage = "必填")]
    public string Subject { get; set; } = null!;

    [Display(Name = "內容")]
    [StringLength(300, MinimumLength = 1, ErrorMessage = "最多300個字")]
    [Required(ErrorMessage = "必填")]
    public string Content { get; set; } = null!;

    [Display(Name = "留言時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd  hh:mm:ss}")]
    [HiddenInput]
    public DateTime Time { get; set; }

    public string TaskID { get; set; } = null!;

    public string? EmployeeID { get; set; }


}
[ModelMetadataType(typeof(MessageData))]
public partial class Message
{
}


public class PositionData
{
    public string PositionNo { get; set; } = null!;

    public string PositionName { get; set; } = null!;

  
}
[ModelMetadataType(typeof(PositionData))]
public partial class Position
{
}


public class TaskData
{
    [Display(Name = "任務編號")]
    [StringLength(8, MinimumLength = 8)]
    [Key]
    public string TaskID { get; set; } = null!;

    [Display(Name = "任務名稱")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "主旨1~50個字")]
    [Required(ErrorMessage = "必填")]
    
    public string TaskName { get; set; } = null!;

    [Display(Name = "建立時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd  hh:mm}", ApplyFormatInEditMode = true)]
    [HiddenInput]
    public DateTime TaskCreateDate { get; set; } = DateTime.Now;

    [Display(Name = "開始日期")]
    [DataType(DataType.DateTime)]
    //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd  hh:mm}")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "必填")]
    public DateTime TaskStartDate { get; set; }

    [Display(Name = "截止日期")]
    [DataType(DataType.DateTime)]
    //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd  hh:mm}")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "必填")]
    public DateTime TaskEndDate { get; set; }


    
    [Display(Name = "狀態")]
    [ForeignKey("StatusNo")]
    [Required(ErrorMessage = "必填")]
    public string StatusNo { get; set; } = null!;


   
    [Display(Name = "類型")]
    [ForeignKey("TaskTypeNo")]
    [Required(ErrorMessage = "必填")]
    public string TaskTypeNo { get; set; } = null!;

    [Display(Name = "小組")]
    [ForeignKey("GroupNo")]
    public string? GroupNo { get; set; }

    [Display(Name = "小組")]
    public virtual Group? GroupNoNavigation { get; set; }    

    [Display(Name = "狀態")]
    //[Required(ErrorMessage = "必填")]
    public virtual TaskStatus? StatusNoNavigation { get; set; }

    [Display(Name = "類型")]
    //[Required(ErrorMessage = "必填")]
    public virtual TaskType? TaskTypeNoNavigation { get; set; }
}
[ModelMetadataType(typeof(TaskData))]
public partial class Task
{
    //可以把分小組跟個人任務加在這邊嗎
    //[NotMapped]
}


public class TaskDetailData
{
    [Key, Column(Order = 0)]
    [ForeignKey("Task")]
    public string TaskID { get; set; } = null!;

    [Key, Column(Order = 1)]
    [ForeignKey("Employee")]
    public string EmployeeID { get; set; } = null!;

    [Display(Name = "任務內容")]
    [Required(ErrorMessage = "必填")]
    [StringLength(300, MinimumLength = 1, ErrorMessage = "最多300個字")]
    public string? TaskContent { get; set; } = null!;

}

[ModelMetadataType(typeof(TaskDetailData))]
public partial class TaskDetail
{
}


public class TaskStatusData
{
    [Display(Name = "狀態編號")]
    [StringLength(1, MinimumLength = 1)]
    [Key]
    public string StatusNo { get; set; } = null!;

    [Display(Name = "狀態名稱")]
    [StringLength(8, MinimumLength = 2)]
    public string Status { get; set; } = null!;

   
}
[ModelMetadataType(typeof(TaskStatusData))]
public partial class TaskStatus
{
}


public class TaskTypeData
{
    [Display(Name = "類型編號")]
    [StringLength(1, MinimumLength = 1)]
    [Key]
    public string TaskTypeNo { get; set; } = null!;

    [Display(Name = "類型名稱")]
    [StringLength(5, MinimumLength = 2)]
    public string TaskTypeName { get; set; } = null!;

}
[ModelMetadataType(typeof(TaskTypeData))]
public partial class TaskType
{
}

public  class ProgressData
{
    [Display(Name = "進度編號")]    
    [Key]
    public int ProgressNo { get; set; }
    [Display(Name = "內容")]
    [StringLength(5, MinimumLength = 2)]
    public string Content { get; set; } = null!;

    [Display(Name = "紀錄日期")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime Time { get; set; }

    public string? TaskID { get; set; } 

    public string? EmployeeID { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Task? Task { get; set; } = null!;
}

[ModelMetadataType(typeof(ProgressData))]
public partial class Progress
{
}