using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MasteryTest3.CustomAttributes {
    public class RedirectSignedOut : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Session.GetInt32("userId") == null) {
                context.Result = new RedirectToActionResult("signin", "auth", null);
            }
            base.OnActionExecuted(context);
        }
    }
}

