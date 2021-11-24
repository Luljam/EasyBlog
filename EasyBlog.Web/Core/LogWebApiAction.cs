using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace EasyBlog.Web.Core
{
    public class LogWebApiAction : ActionFilterAttribute
    {
        public ILogger _Logger { private get; set; }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            _Logger.Log(actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
        }
    }
}