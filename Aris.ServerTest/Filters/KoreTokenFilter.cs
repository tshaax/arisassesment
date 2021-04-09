using System;
using System.Threading.Tasks;
using Aris.ServerTest.Models;
using Aris.ServerTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Aris.ServerTest.Filters
{
    public class KoreTokenFilter : IAsyncAuthorizationFilter, IResultFilter
    {
        private const string KoreAuthCookieKey = "kore-api-auth";
        private readonly IKoreApiAuthService _koreApiAuthService;
        private readonly IKoreClaimsService _koreClaims;
        private readonly KoreOptions _koreOptions;

        public KoreTokenFilter(IKoreApiAuthService koreAuthService, IKoreClaimsService koreClaims, IOptions<KoreOptions> koreOptions)
        {
            _koreApiAuthService = koreAuthService;
            _koreOptions = koreOptions.Value;
            _koreClaims = koreClaims;
        }

        /// <summary>
        /// Gets the Kore API bearer token from the cookie, refreshes if needed and stores as identity claim
        /// </summary>
        /// <remarks>
        /// First step is to check the cookie from the request, then:
        ///
        /// * If there is a player token then check if it needs to be refreshed
        /// 
        /// * If there is a public brand token then check if it has expired and get a new one
        /// 
        /// * If there is no token then get a new public brand token
        /// 
        /// Finally, store the token as an identity claim
        /// </remarks>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            KoreAuthToken token = null;
            HttpRequest httpRequest = context.HttpContext.Request;
            string returnUrl = ReturnUrlActionFilter.GetReturnUrl(httpRequest);

            if (httpRequest.Cookies[KoreAuthCookieKey] != null)
            {
                token = JsonConvert.DeserializeObject<KoreAuthToken>(httpRequest.Cookies[KoreAuthCookieKey], new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });

                if (token.Expiration <= DateTime.UtcNow && token.GrantType == KoreAuthToken.Player)
                {
                    token = null;
                }
            }

            // get a new public brand token
            if (token == null)
            {
                token = await _koreApiAuthService.GetPublicBrandTokenAsync(returnUrl, _koreOptions.Brand, _koreOptions.Locale);
            }

            _koreClaims.AddTokenToIdentityClaims(token, context.HttpContext.User);
        }

        protected void SetCookie(HttpResponse httpResponse, KoreAuthToken token)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            var json = JsonConvert.SerializeObject(token, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
            httpResponse.Cookies.Append(KoreAuthCookieKey, json, options);
        }

        /// <summary>
        /// Gets the user claims and saves this to the cookie
        /// </summary>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            //set the cookie in here
            if (!context.HttpContext.User.HasClaim(cl => cl.Type == "TokenType"))
            {
                throw new NullReferenceException("Token not set");
            }

            SetCookie(context.HttpContext.Response, KoreAuthToken.GetAuthTokenFromClaims(context.HttpContext.User));
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // do nothing (required for IResultFilter interface)

        }
    }
}
