using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aris.ServerTest.ViewModels;
using Aris.ServerTest.Filters;

namespace Aris.ServerTest.Controllers
{
    public class AccountController : BaseController
    {
        private readonly Services.IKoreClaimsService _koreClaimsService;
        private readonly Services.IKoreApiUserService _koreUserService;

        public AccountController(Services.IKoreClaimsService koreClaimsService,
            Services.IKoreApiUserService koreUserService)
        {
            _koreClaimsService = koreClaimsService;
            _koreUserService = koreUserService;
        }

        [ReturnUrlFromReferrer]
        public async Task<IActionResult> Register(string returnUrl)
        {
            // we must set the returnUrl to the referrer and not the requestUrl
            // otherwise we may get stuck in a redirect loop
            var userLinks =  await _koreUserService.GetUserLinksAsync(GetAuthToken(), returnUrl);
       
            if (!userLinks.Actions.ContainsKey(Models.KoreUserLinks.RegisterLink))
            {
                throw new ApplicationException("Error getting registration link from API");
            }

            return Redirect(userLinks.Actions[Models.KoreUserLinks.RegisterLink].Url);
        }

        [ReturnUrlFromReferrer]
        public async Task<IActionResult> ResetPassword(string returnUrl)
        {
            var userLinks = await _koreUserService.GetUserLinksAsync(GetAuthToken(), returnUrl);

            if (!userLinks.Actions.ContainsKey(Models.KoreUserLinks.ResetPasswordLink))
            {
                throw new ApplicationException("Error getting reset password link from API");
            }

            return Redirect(userLinks.Actions[Models.KoreUserLinks.ResetPasswordLink].Url);
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReturnUrlFromRequest]
        public async Task<IActionResult> Login(ViewModels.LoginViewModel loginViewModel, string returnUrl)
        {
            throw new NotImplementedException();
        }

        [ReturnUrlFromRequest]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            throw new NotImplementedException();
        }
    }
}
