using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Teamwork.Filters;
using Teamwork.Models;
using Teamwork.ViewModels;

namespace Teamwork.Controllers
{
    [ServiceFilter(typeof(LoginStatusFilter))]
    public class GroupsController : Controller
    {
        private readonly TeamworkContext _context;

        public GroupsController(TeamworkContext context)
        {
            // _context = context;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            
            string groupid = await _context.Group.Select(g => g.GroupNo).FirstOrDefaultAsync();
           

            VMGroup groups = new VMGroup()
            {
                Groups = await _context.Group
            .Include(g => g.GroupDetail)  // 加入 GroupDetail
            .Include(g => g.Task)         // 加入 Task
            .ToListAsync(),
                Tasks = await _context.Task.Where(t => t.GroupNo == groupid).ToListAsync()
                
            };

            //ViewData["DeptName"] = _context.Department.Find(deptid).DeptName;
            //ViewData["DeptID"] = deptid;

            return View(groups);
            //return View(await _context.Group.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Task) // 加入這行，確保載入 Task
                .FirstOrDefaultAsync(m => m.GroupNo == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            string employeeId = HttpContext.Session.GetString("NEmployee");

            // 初始化 VMGroup，並預選 NEmployee
            var vmgroup = new VMGroup
            {
                EmployeeIDs = new List<string> { employeeId } // 預選 NEmployee
            };

            // 準備員工列表，排除 NEmployee
            ViewBag.Employees = _context.Employee
                .Where(e => e.EmployeeID != employeeId) // 排除 NEmployee
                .Select(e => new SelectListItem
                {
                    Value = e.EmployeeID.ToString(),
                    Text = e.EmployeeName
                })
                .ToList();

            ViewBag.NEmployee = employeeId;

            //return View();
            return View(vmgroup);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMGroup vmgroup)
        {
            string employeeId = HttpContext.Session.GetString("NEmployee");

            if (vmgroup.Group == null)
            {
                vmgroup.Group = new Models.Group();
            }
            if (vmgroup.GroupDetail == null)
            {
                vmgroup.GroupDetail = new GroupDetail();
            }

            vmgroup.Group.GroupNo = GenerateGroupNo();
             Console.WriteLine("Generated TaskID: " + vmgroup.Group.GroupNo);
            // 確保 GroupNo 被正確設定
            if (string.IsNullOrEmpty(vmgroup.Group.GroupNo))
            {
                vmgroup.Group.GroupNo = GenerateGroupNo();
            }

            // 確保 EmployeeIDs 包含 NEmployee
            if (vmgroup.EmployeeIDs == null)
            {
                vmgroup.EmployeeIDs = new List<string>();
            }
            if (!vmgroup.EmployeeIDs.Contains(employeeId))
            {
                vmgroup.EmployeeIDs.Add(employeeId); // 強制添加 NEmployee
            }

            // 檢查選擇的員工 ID 是否有效
            if (vmgroup.EmployeeIDs == null || vmgroup.EmployeeIDs.Count == 1)
            {
                ModelState.AddModelError("", "請至少選擇一名員工！");
            }
            
            

            ModelState.Remove("Group.GroupNo");
            ModelState.Remove("GroupDetail.GroupNo");
            ModelState.Remove("GroupDetail.EmployeeID");
            ModelState.Remove("GroupDetail.Teamleader");

            if (ModelState.IsValid)
            {
                
                var group = new Models.Group
                {
                    GroupNo = vmgroup.Group.GroupNo,
                    GroupName = vmgroup.Group.GroupName,
                    StartDate = vmgroup.Group.StartDate,
                    EndDate = vmgroup.Group.EndDate,
                    
                };

                _context.Group.Add(group);
                await _context.SaveChangesAsync();

                if (vmgroup.EmployeeIDs != null && vmgroup.EmployeeIDs.Count > 0)
                {
                    foreach (var empId in vmgroup.EmployeeIDs)
                    {
                        var groupDetail = new GroupDetail
                        {
                            GroupNo = vmgroup.Group.GroupNo,
                            Teamleader = employeeId,
                            GroupContent = vmgroup.GroupDetail.GroupContent,
                            EmployeeID = empId

                        };
                        _context.GroupDetail.Add(groupDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                
                //await _context.SaveChangesAsync();

                //if (vmgroup.EmployeeIDs != null && vmgroup.EmployeeIDs.Count > 0)
                //{
                //    foreach (var empId in vmgroup.EmployeeIDs)
                //    {
                //        var newGroupDetail = new GroupDetail
                //        {
                //            GroupNo = group.GroupNo,
                //            Teamleader = employeeId,
                //            GroupContent = vmgroup.GroupDetail.GroupContent,
                //            EmployeeID = empId
                //        };

                //        _context.GroupDetail.Add(newGroupDetail);
                //    }
                //    await _context.SaveChangesAsync();
                //}
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = _context.Employee
                    .Select(e => new SelectListItem
                    {
                        Value = e.EmployeeID.ToString(),
                        Text = e.EmployeeName
                    })
                    .ToList();

                //return View(vmgroup);
            }
            return View(vmgroup);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 1. 使用 Include 載入 GroupDetail，確保它有值
            var group = await _context.Group
                .Include(g => g.GroupDetail)
                .FirstOrDefaultAsync(g => g.GroupNo == id);

            if (group == null)
            {
                return NotFound();
            }

            // 2. 確保 StartDate 不為預設值
            if (group.StartDate == default(DateTime) || group.StartDate == DateTime.MinValue)
            {
                group.StartDate = DateTime.Now;
            }

            // 3. 檢查 GroupDetail 是否為 null 或空集合，若是則初始化一個新的
            if (group.GroupDetail == null || !group.GroupDetail.Any())
            {
                group.GroupDetail = new List<GroupDetail>
                {
                    new GroupDetail
                    {
                        GroupNo = group.GroupNo,
                        GroupContent = "未填寫"  // 預設值
                    }
                };
            }

            // 4.確保 GroupContent 如果是 null 或空白，設為 "未填寫"
            var groupDetail = group.GroupDetail.First();
            if (string.IsNullOrWhiteSpace(groupDetail.GroupContent))
            {
                groupDetail.GroupContent = "未填寫";
            }

            // 5. 建立 ViewModel 傳遞資料到 View
            var vmGroup = new VMGroup
            {
                Group = group,
                GroupDetail = groupDetail
            };

            return View(vmGroup);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, VMGroup vmgroup)
        {
            if (id != vmgroup.Group.GroupNo)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    var task = await _context.Task.FindAsync(id);
                    _context.Update(vmgroup.Group);// 更新 Group 實體而非 VMGroup
                    // 查詢並更新對應的 GroupDetail
                    var groupDetail = await _context.GroupDetail
                        .FirstOrDefaultAsync(gd => gd.GroupNo == vmgroup.Group.GroupNo);
                    
                    groupDetail.GroupContent = vmgroup.GroupDetail?.GroupContent?.Trim() ?? groupDetail.GroupContent;
                    _context.Update(groupDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(vmgroup.Group.GroupNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vmgroup);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Task) // 假設 Group 和 Task 有關聯
                .FirstOrDefaultAsync(m => m.GroupNo == id);

            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            //// 先刪除與 Group 相關的 GroupDetail 資料
            //var groupDetails = _context.GroupDetail.Where(gd => gd.GroupNo == id);
            //_context.GroupDetail.RemoveRange(groupDetails); // 刪除所有 GroupDetail 相關的資料

            //// 然後刪除 Group 資料
            //var group = await _context.Group.FindAsync(id);
            //if (group != null)
            //{
            //    _context.Group.Remove(group);
            //}

            //await _context.SaveChangesAsync();

            var group = await _context.Group
                .Include(g => g.Task) // 確保加載 Task
                .FirstOrDefaultAsync(g => g.GroupNo == id);

            if (group == null)
            {
                return NotFound();
            }

            // 檢查是否仍有任務
            if (group.Task != null && group.Task.Any())
            {
                ViewBag.DeleteError = "請先刪除小組內的任務，才能刪除小組。";
                return View("Delete", group); // 回到刪除頁面
            }

            // 刪除 GroupDetail
            var groupDetails = _context.GroupDetail.Where(gd => gd.GroupNo == id);
            _context.GroupDetail.RemoveRange(groupDetails);

            // 刪除 Group
            _context.Group.Remove(group);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(string id)
        {
            return _context.Group.Any(e => e.GroupNo == id);
        }






        ///////////////


        private string GenerateGroupNo()
        {
            // 假設你要的 ID 格式為 "2502"（例如 25 是年份後兩碼，02 是月份）
            int year = DateTime.Now.Year % 100; // 取西元年後兩碼
            string baseGroupNo = $"G{year:D2}";

            // 取得最新的群組編號
            var latestGroup = _context.Group
                .Where(g => g.GroupNo.StartsWith(baseGroupNo)) // 只篩選當年前綴的
                .OrderByDescending(g => g.GroupNo)
                .FirstOrDefault();



            int newSequence = 1; // 預設從 0001 開始
            if (latestGroup != null)
            {
                string sequencePart = latestGroup.GroupNo.Substring(3); // 取得流水號部分
                if (int.TryParse(sequencePart, out int latestSequence))
                {
                    newSequence = latestSequence + 1;
                }
            }

            return $"{baseGroupNo}{newSequence:D4}"; // 確保流水號為 4 碼
        }


        //public bool EmployeeGroupNameCheck(string name)
        //{

        //    var result = _context.Group.Where(g => g.GroupName == name).FirstOrDefault();
        //    //VMGroup.Delay(2000).Wait();

        //    return result == null;
        //}

        [HttpGet]
        public JsonResult EmployeeGroupNameCheck(string name)
        {
            bool isAvailable = !_context.Group.Any(g => g.GroupName == name);
            return Json(isAvailable);
        }
    }
}
