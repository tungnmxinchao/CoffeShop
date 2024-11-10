using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CoffeShop.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireManagerAttribute : Attribute, IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {

        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {

            var role = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(role) ||
                (role != "Staff" && role != "Admin"))
            {
                context.Result = new RedirectToPageResult("/CoffeApp/Login");
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }
    }
}
