using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Teamwork.Models;

namespace Teamwork.ViewModels
{
    public class VMTask
    {
        public VMTask()
        {
            Task = new Models.Task(); // 初始化 Task
            TaskDetail = new TaskDetail(); // 初始化 TaskDetail
        }

        public Models.Task Task { get; set; }        

        public TaskDetail TaskDetail { get; set; }

    }

}
