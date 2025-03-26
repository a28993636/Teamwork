using Teamwork.Models;

namespace Teamwork.ViewModels
{
    public class VMGroup
    {


        public VMGroup()
        {
            Group = new Models.Group(); // 初始化 Task
            GroupDetail = new GroupDetail(); // 初始化 TaskDetail
        }

        public Group Group { get; set; }

        public GroupDetail GroupDetail { get; set; }

        public List<string> EmployeeIDs { get; set; } = new List<string>(); // 允許多選

        public List<Group>? Groups { get; set; }
        public List<Models.Task>? Tasks { get; set; }

    }
}
