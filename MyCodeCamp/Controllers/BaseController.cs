using System;
using Microsoft.AspNetCore.Mvc;

namespace MyCodeCamp
{
    public abstract class BaseController : Controller
    {
        public const string URLHELPER = "URLHELPER";

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            context.HttpContext.Items[URLHELPER] = this.Url;
        }
    }
}
