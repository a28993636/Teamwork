using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teamwork.Filters;
using Teamwork.Models;
using Teamwork.ViewModels;

namespace Teamwork.Controllers
{
    [ServiceFilter(typeof(LoginStatusFilter))]
    public class ProgressesController : Controller
    {
        private readonly TeamworkContext _context;

        public ProgressesController(TeamworkContext context)
        {
            _context = context;
        }

        // GET: Progresses
        public async Task<IActionResult> Index(string? id, string returnUrl = null)
        {
            if (id == null)
            {
                return NotFound(); // 如果沒有提供 id，返回 404
            }

            // 查詢指定 TaskID 的進度紀錄
            var progresses = await _context.Progress
                .Include(p => p.Employee)
                .Include(p => p.Task)
                .Where(p => p.TaskID == id)
                .ToListAsync();

            // 設置 HasRecords 來檢查是否有進度紀錄
            ViewData["HasRecords"] = progresses.Any();
            ViewData["TaskID"] = id; // 傳遞 TaskID，便於後續操作

            // 如果 returnUrl 有效，返回前一頁；否則回到 Tasks 的 Index 頁面
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            else
            {
                ViewBag.ReturnUrl = Url.Action("Index", "Tasks");
            }

            return View(progresses);
        }


        // GET: Progresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progress = await _context.Progress
                .Include(p => p.Employee)
                .Include(p => p.Task)
                .FirstOrDefaultAsync(m => m.ProgressNo == id);
            if (progress == null)
            {
                return NotFound();
            }

            return View(progress);
        }

        // GET: Progresses/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID");
            ViewData["TaskID"] = new SelectList(_context.Task, "TaskID", "TaskID");
            return View();
        }

        // POST: Progresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Progress progress)
        {
            ModelState.Remove("Task");
            ModelState.Remove("TaskID");
            //TaskID = vmtask.Task.TaskID;

            if (ModelState.IsValid)
            {
                _context.Add(progress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", progress.EmployeeID);
            ViewData["TaskID"] = new SelectList(_context.Task, "TaskID", "TaskID", progress.TaskID);
            return View(progress);
        }

        // GET: Progresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progress = await _context.Progress.FindAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", progress.EmployeeID);
            ViewData["TaskID"] = new SelectList(_context.Task, "TaskID", "TaskID", progress.TaskID);
            return View(progress);
        }

        // POST: Progresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProgressNo,Content,Time,TaskID,EmployeeID")] Progress progress)
        {
            if (id != progress.ProgressNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgressExists(progress.ProgressNo))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "EmployeeID", progress.EmployeeID);
            ViewData["TaskID"] = new SelectList(_context.Task, "TaskID", "TaskID", progress.TaskID);
            return View(progress);
        }

        // GET: Progresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progress = await _context.Progress
                .Include(p => p.Employee)
                .Include(p => p.Task)
                .FirstOrDefaultAsync(m => m.ProgressNo == id);
            if (progress == null)
            {
                return NotFound();
            }

            return View(progress);
        }

        // POST: Progresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progress = await _context.Progress.FindAsync(id);
            if (progress != null)
            {
                _context.Progress.Remove(progress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgressExists(int id)
        {
            return _context.Progress.Any(e => e.ProgressNo == id);
        }
    }
}
