using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Teamwork.Filters;
using Teamwork.Models;
using Teamwork.ViewModels;
using Task = Teamwork.Models.Task;

namespace Teamwork.Controllers
{
    [ServiceFilter(typeof(LoginStatusFilter))]
    public class TasksController : Controller
    {
        private readonly TeamworkContext _context;

        public TasksController(TeamworkContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        
        // GET: Tasks
        public async Task<IActionResult> IndexAll(string employeeId)
        {
            //employeeId = HttpContext.Session.GetString("NEmployee"); // 取出 Session

            if (string.IsNullOrEmpty(employeeId))
            {
                return RedirectToAction("Login", "Login"); // 若未登入，跳轉至登入頁面
            }

            //_context.TaskDetail.Where(td => td.EmployeeID == Id).ToList();
            var teamworkContext = _context.Task.Include(t => t.GroupNoNavigation)
                .Include(t => t.StatusNoNavigation)
                .Include(t => t.TaskTypeNoNavigation)
                .Include(t => t.TaskDetail)
                .Where(t => t.TaskDetail.Any(td => td.EmployeeID == employeeId)); // 篩選條件; !!待確認？？？

            return View(await teamworkContext.ToListAsync());
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string employeeId)
        {
            //employeeId = HttpContext.Session.GetString("NEmployee"); // 取出 Session

            if (string.IsNullOrEmpty(employeeId))
            {
                return RedirectToAction("Login", "Login"); // 若未登入，跳轉至登入頁面
            }

            //_context.TaskDetail.Where(td => td.EmployeeID == Id).ToList();
            var teamworkContext = _context.Task.Include(t => t.GroupNoNavigation)
                .Include(t => t.StatusNoNavigation)
                .Include(t => t.TaskTypeNoNavigation)
                .Include(t => t.TaskDetail)
                .Where(t => t.TaskDetail.Any(td => td.EmployeeID == employeeId))
                .Where(t => t.GroupNo == null);
                //.Where(t => t.GroupNoNavigation.Equals(td => td.GroupNo == "--")); // 篩選條件; !!待確認？？？

            return View(await teamworkContext.ToListAsync());
        }

        
        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.GroupNoNavigation)
                .Include(t => t.StatusNoNavigation)
                .Include(t => t.TaskTypeNoNavigation)
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create(string? groupNo,string returnUrl  )
        {
             var viewModel = new VMTask();
            if (string.IsNullOrEmpty(groupNo))
            {
                viewModel.Task = new Task
                {
                    GroupNo = groupNo
                };
                ViewBag.IsGroupTask = true;
            }
            else
            {
                ViewBag.IsGroupTask = false;
            }



            ViewData["Group"] = new SelectList(_context.Group, "GroupNo", "GroupName");
            ViewData["Status"] = new SelectList(_context.TaskStatus, "StatusNo", "Status");
            ViewData["TaskType"] = new SelectList(_context.TaskType, "TaskTypeNo", "TaskTypeName");
            ViewBag.ReturnUrl = returnUrl; // 將 returnUrl 存入 ViewBag
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMTask vmtask, string returnUrl)
        {
            string employeeId = HttpContext.Session.GetString("NEmployee");

            // 確保 Task 和 TaskDetail 不為 null
            if (vmtask.Task == null)
            {
                vmtask.Task = new Models.Task();
            }
            if (vmtask.TaskDetail == null)
            {
                vmtask.TaskDetail = new TaskDetail();
            }
            ModelState.Remove("Task.TaskID");
            ModelState.Remove("TaskDetail.TaskID");
            ModelState.Remove("TaskDetail.EmployeeID");

            vmtask.Task.TaskID = GenerateTaskID();
            Console.WriteLine($"Employee ID: {employeeId}");

            if (string.IsNullOrEmpty(employeeId))
            {
                return RedirectToAction("Login", "Login"); // 若未登入，跳轉至登入頁面
            }

            // 檢查 TaskName 是否已經存在
            if (_context.Task.Any(t => t.TaskName == vmtask.Task.TaskName))
            {
                ModelState.AddModelError("Task.TaskName", "任務名稱已經存在，請使用不同的名稱。");
            }

            if (ModelState.IsValid)
            { 
                var task = new Models.Task
                {
                    TaskID = vmtask.Task.TaskID,
                    TaskName = vmtask.Task.TaskName, 
                    TaskStartDate = vmtask.Task.TaskStartDate,
                    TaskEndDate = vmtask.Task.TaskEndDate,
                    StatusNo = vmtask.Task.StatusNo,
                    TaskTypeNo = vmtask.Task.TaskTypeNo,
                    GroupNo = vmtask.Task.GroupNo
                };

                _context.Task.Add(task);
                await _context.SaveChangesAsync();

                var taskDetail = new TaskDetail
                {
                    TaskID = vmtask.Task.TaskID, // 來自剛剛存入的 Task
                    EmployeeID = employeeId, // 設定 EmployeeID
                    TaskContent = vmtask.TaskDetail.TaskContent // 或來自表單
                };
              
                _context.TaskDetail.Add(taskDetail);
                await _context.SaveChangesAsync(); // 儲存 TaskDetail

                // 如果 Task.GroupNo 有值，強制回到 IndexAll
                if (!string.IsNullOrEmpty(task.GroupNo))
                {
                    return RedirectToAction("IndexAll", "Tasks", new { employeeId });
                }

                // 如果 returnUrl 有效，返回前一頁；否則預設回到 Index
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("IndexALL", "Tasks", new { employeeId });
                }
                //return RedirectToAction("IndexAll", "Tasks", employeeId);
            }

            

            ViewData["Group"] = new SelectList(_context.Group, "GroupNo", "GroupName", vmtask.Task.GroupNo);
            ViewData["Status"] = new SelectList(_context.TaskStatus, "StatusNo", "Status", vmtask.Task.StatusNo);
            ViewData["TaskType"] = new SelectList(_context.TaskType, "TaskTypeNo", "TaskTypeName", vmtask.Task.TaskTypeNo);
            ViewBag.ReturnUrl = returnUrl; // 若表單驗證失敗，將 returnUrl 傳回視圖
            return View(vmtask);
            
            
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            //string employeeid = HttpContext.Session.GetString("NEmployee"); // 從 Session 獲取
            if (id == null )
            {
                return NotFound();
            }
           
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            // 查詢相關的 TaskDetail 資料
        var taskDetail = await _context.TaskDetail
            .Where(td => td.TaskID == id)
            .FirstOrDefaultAsync();

            // 創建並填充 VMTask 物件
            var vmTask = new VMTask
            {
                Task = task,
                TaskDetail = taskDetail ?? new TaskDetail() // 如果 TaskDetail 不存在，初始化一個空的物件
            };

            ViewData["GroupNo"] = new SelectList(_context.Group, "GroupNo", "GroupName", vmTask.Task.GroupNo);
            ViewData["StatusNo"] = new SelectList(_context.TaskStatus, "StatusNo", "Status", vmTask.Task.StatusNo);
            ViewData["TaskTypeNo"] = new SelectList(_context.TaskType, "TaskTypeNo", "TaskTypeName", vmTask.Task.TaskTypeNo);

            // 設置初始任務類型（個人或小組）
            if (vmTask.Task.GroupNo != null)
            {
                ViewBag.TaskPG = "group"; // 小組任務
            }
            else
            {
                ViewBag.TaskPG = "personal"; // 個人任務
            }

            return View(vmTask);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, VMTask vmtask, string returnUrl = null)
        {
            var employeeId = HttpContext.Session.GetString("NEmployee");
            if (string.IsNullOrEmpty(employeeId))
            {
                Console.WriteLine("Session 已過期或未設置");
                return RedirectToAction("Login", "Login");
            }

            if (id != vmtask.Task.TaskID)
            {
                return NotFound();
            }
            // 檢查 TaskName 是否已經存在（排除當前任務本身）
            if (_context.Task.Any(t => t.TaskName == vmtask.Task.TaskName && t.TaskID != id))
            {
                ModelState.AddModelError("Task.TaskName", "任務名稱已經存在，請使用不同的名稱。");
            }
            
            
            if (ModelState.IsValid)
            {
                try
                {
                    //if (string.IsNullOrEmpty(vmtask.Task.GroupNo) || vmtask.Task.GroupNo == "NULL")
                    //{
                    //    vmtask.Task.GroupNo = null;
                    //}

                    //_context.Update(vmtask);
                    //await _context.SaveChangesAsync();

                    // 從資料庫中查詢現有的 Task 和 TaskDetail
                    var task = await _context.Task.FindAsync(id);
                    var taskDetail = await _context.TaskDetail.FirstOrDefaultAsync(td => td.TaskID == id);

                    if (task == null)
                    {
                        return NotFound();
                    }

                    // 更新 Task 的屬性
                    task.TaskName = vmtask.Task.TaskName;
                    task.TaskCreateDate = task.TaskCreateDate;
                    task.TaskStartDate = vmtask.Task.TaskStartDate;
                    task.TaskEndDate = vmtask.Task.TaskEndDate;
                    task.StatusNo = vmtask.Task.StatusNo;
                    task.TaskTypeNo = vmtask.Task.TaskTypeNo;
                    task.GroupNo = string.IsNullOrEmpty(vmtask.Task.GroupNo) || vmtask.Task.GroupNo == "NULL" ? null : vmtask.Task.GroupNo;

                    // 如果 TaskDetail 存在，更新它的屬性
                    if (taskDetail != null && vmtask.TaskDetail != null)
                    {
                        taskDetail.TaskContent = vmtask.TaskDetail.TaskContent;
                        taskDetail.TaskID = taskDetail.TaskID;
                        taskDetail.EmployeeID = taskDetail.EmployeeID;

                    }

                    await _context.SaveChangesAsync();

                    // 如果 Task.GroupNo 有值，強制回到 IndexAll
                    if (!string.IsNullOrEmpty(task.GroupNo))
                    {
                        return RedirectToAction("IndexAll", "Tasks", new { employeeId });
                    }                    
                    else if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))// 如果 returnUrl 有效，返回前一頁；否則預設回到 Index
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("IndexALL", "Tasks", new { employeeId });
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(vmtask.Task.TaskID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return Redirect(returnUrl);
                //return RedirectToAction(nameof(Index));
            }
            ViewData["GroupNo"] = new SelectList(_context.Group, "GroupNo", "GroupNo", vmtask.Task.GroupNo);
            ViewData["StatusNo"] = new SelectList(_context.TaskStatus, "StatusNo", "StatusNo", vmtask.Task.StatusNo);
            ViewData["TaskTypeNo"] = new SelectList(_context.TaskType, "TaskTypeNo", "TaskTypeNo", vmtask.Task.TaskTypeNo);
            //return View(vmtask);
            return Redirect(returnUrl);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //string employeeid = HttpContext.Session.GetString("NEmployee"); // 從 Session 獲取
            //if (id == null || employeeid == null)
            //{
            //    return NotFound();
            //}

            //var task = await _context.TaskDetail
            //    .Include(t => t.TaskID)
            //    .Include(t => t.EmployeeID)
            //    .FirstOrDefaultAsync(m => m.TaskID == id && m.EmployeeID == employeeid);

            var task = await _context.Task
                .Include(t => t.GroupNoNavigation)
                .Include(t => t.StatusNoNavigation)
                .Include(t => t.TaskTypeNoNavigation)               
                .FirstOrDefaultAsync(m => m.TaskID == id);
            var taskdetail = await _context.TaskDetail
                .Include(t => t.Task)                
                .FirstOrDefaultAsync(m => m.TaskID == id);
            if (task == null)
            {
                return NotFound();
            }

            var vmTask = new VMTask
            {
                Task = task,
                TaskDetail = taskdetail
            };

            return View(vmTask); // 傳遞 VMTask 物件給視圖

            
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                string employeeId = HttpContext.Session.GetString("NEmployee");
                var task = await _context.Task.FindAsync(id);
                var taskDetail = await _context.TaskDetail.FindAsync(id, employeeId);

                if (task == null || taskDetail == null)
                {
                    return NotFound();
                }

                _context.TaskDetail.Remove(taskDetail);
                _context.Task.Remove(task);
                Console.WriteLine($"刪除成功");
                await _context.SaveChangesAsync();
                return Json(new { success = true }); // 返回 JSON 給 AJAX
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Post]DeleteConfirmed刪除失敗: {ex.Message}");
                return StatusCode(500, new { success = false, message = "刪除失敗" });
            }
        }

        private bool TaskExists(string id)
        {
            return _context.Task.Any(e => e.TaskID == id);
        }


        public IActionResult Logout()
        {
            //5.4.2 在Logout Action中清除Session
            //HttpContext.Session.Clear();//清掉所有的Session
            HttpContext.Session.Remove("NEmployee");//清掉Manager的Session

            return RedirectToAction("Index", "Home");
        }


        ///////////////


        private string GenerateTaskID()
        {
            // 假設你要的 ID 格式為 "2502"（例如 25 是年份後兩碼，02 是月份）
            int year = DateTime.Now.Year % 100; // 取西元年後兩碼
            int month = DateTime.Now.Month; // 取當前月份
            string taskID = $"{year:D2}{month:D2}"; // 組合成 2502

            var latestTaskID = _context.Task
            .OrderByDescending(t => t.TaskID) // 假設 TaskID 的格式是正確的，能夠按字典順序排列
            .FirstOrDefault()?.TaskID;

            

            // 檢查最新的任務編號以確保唯一性
            if (latestTaskID != null && latestTaskID.StartsWith(taskID))
            {
                // 取得 latestTaskID 的最後 4 碼
                string sequencePart = latestTaskID.Substring(4); // 從第 5 個字元開始取剩下的部分

                if (int.TryParse(sequencePart, out int latestSequence))
                {
                    int newSequence = latestSequence + 1; // 增加 1
                    taskID = $"{year:D2}{month:D2}{newSequence:D4}"; // 確保 4 碼格式
                }
                else
                {
                    taskID += "0001"; // 如果解析失敗，則從 0001 開始
                }
            }
            else
            {
                // 如果 latestTaskID 為 null 或不符合當前年月，則從 0001 開始
                taskID += "0001";
            }

            return taskID; // 返回生成的任務編號
        }

        // 在 TasksController 中添加
        [HttpGet]
        public IActionResult CheckTaskName(string taskName, string taskId = null)
        {
            // 如果 taskId 不為空（編輯模式），排除當前任務本身
            bool exists = string.IsNullOrEmpty(taskId)
                ? _context.Task.Any(t => t.TaskName == taskName)
                : _context.Task.Any(t => t.TaskName == taskName && t.TaskID != taskId);

            return Json(new { exists });
        }
    }
}
