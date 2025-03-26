using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Teamwork.Models;

namespace Teamwork.Filters
{
    public class LoginStatusFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //檢查使用者是否已登入(Session是否有值)
            //if (context.HttpContext.Session.GetString("MemberInfo") == null)
            //{
            //    //尚未登入,導向登入頁面
            //    context.Result = new RedirectToActionResult("Login", "Login", null);
            //}

            var employeeId = context.HttpContext.Session.GetString("NEmployee"); // 取出 Session

            if (string.IsNullOrEmpty(employeeId))
            {
                context.Result = new RedirectToActionResult("Login", "Login", null); // 若未登入，跳轉至登入頁面
            }
            else {
                // 將 employeeId 傳遞給 Controller 方法
                context.ActionArguments["employeeId"] = employeeId;
            }
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
