using Aris.ServerTest.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Aris.ServerTest.Models;

namespace Aris.ServerTest.Controllers
{
    public abstract class BaseController : Controller
    {
  

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = exceptionHandlerPathFeature.Error.Message
            });
        }

        public KoreAuthToken GetAuthToken()
        {
            return KoreAuthToken.GetAuthTokenFromClaims(User);
        }
    }
}