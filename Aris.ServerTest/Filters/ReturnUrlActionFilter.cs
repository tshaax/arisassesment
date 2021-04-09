using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Aris.ServerTest.Filters
{
    public class ReturnUrlFromRequestAttribute : ActionFilterAttribute
    {

    }

    public class ReturnUrlFromReferrerAttribute : ActionFilterAttribute
    {

    }

    public class ReturnUrlActionFilter : IActionFilter
    {

        public static string GetReturnUrl(HttpRequest request)
        {
            return string.Concat(
                request.Scheme,
                "://",
                request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                request.Path.ToUriComponent(),
                request.QueryString.ToUriComponent());
        }

        public static string GetReturnUrlFromReferrer(HttpRequest request)
        {
            string referrer = request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referrer))
            {
                throw new NullReferenceException("Referrer not found!");
            }

            return referrer;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var allowFilters = context?.ActionDescriptor?.FilterDescriptors;

            if (allowFilters != null)
            {
                if (allowFilters.Count(flt => flt.Filter is ReturnUrlFromRequestAttribute) > 0)
                {
                    context.ActionArguments["returnUrl"] = GetReturnUrl(context.HttpContext.Request);
                    return;
                }

                if (allowFilters.Count(flt => flt.Filter is ReturnUrlFromReferrerAttribute) > 0)
                {
                    context.ActionArguments["returnUrl"] = GetReturnUrlFromReferrer(context.HttpContext.Request);
                }

            }
        }
    }
}