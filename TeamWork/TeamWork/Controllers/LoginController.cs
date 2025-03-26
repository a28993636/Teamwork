using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Teamwork.Models;

namespace Teamwork.Controllers
{
    public class LoginController : Controller
    {
        private readonly TeamworkContext _context;
        public LoginController(TeamworkContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Account login)
        {
            string sql = "select * from Account where AccountID = @Account and Password = @Password";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Account", login.AccountID),
                new SqlParameter("@Password", login.Password)
            };

            var result = await _context.Account.FromSqlRaw(sql, parameters).FirstOrDefaultAsync();

            //如果帳密正確,導入後台頁面
            if (result != null)
            {
                //在LoginController的Post Login Action中加入Session紀錄登入狀態
                //發給證明,證明他已經登入
                HttpContext.Session.SetString("NEmployee", result.AccountID);

                // 查詢 Employee
                var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeID == result.AccountID);
                if (employee != null)
                {
                    HttpContext.Session.SetString("EmployeeName", employee.EmployeeName);
                }

                return RedirectToAction("IndexAll", "Tasks");
            }
            else //如果帳密不正確,回到登入頁面,並告知帳密錯誤
            {
                ViewData["Message"] = "帳號或密碼錯誤";
            }

            return View(login);
        }

       


        //5.4.1 在LoginController加入Logout Action
        public IActionResult Logout()
        {
            //5.4.2 在Logout Action中清除Session
            //HttpContext.Session.Clear();//清掉所有的Session
            HttpContext.Session.Remove("NEmployee");//清掉Manager的Session

            return RedirectToAction("Index", "Home");
        }
    }
}
