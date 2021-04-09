using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Aris.ServerTest.Models;
using Microsoft.Extensions.Options;

namespace Aris.ServerTest.Services
{
    public class KoreClaimsService : IKoreClaimsService
    {
        private readonly IKoreApiAuthService _koreAuthService;
        private readonly KoreOptions _koreOptions;

        public KoreClaimsService(IKoreApiAuthService koreAuthService, IOptions<KoreOptions> koreOptions)
        {
            _koreAuthService = koreAuthService;
            _koreOptions = koreOptions.Value;
        }

        public void AddTokenToIdentityClaims(KoreAuthToken token, ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;

            // if for any reason the same token already exists as a claim then exit
            if (identity.HasClaim(cl => cl.Type == "Value" && cl.Value == token.Value))
            {
                return;
            }

            var claims = new List<Claim>();
            claims.Add(new Claim("TokenType", token.TokenType));
            claims.Add(new Claim("GrantType", token.GrantType));
            claims.Add(new Claim("Value", token.Value));
            claims.Add(new Claim("Expiration", token.Expiration.ToString()));
            claims.Add(new Claim("TrackingId", token.TrackingId ?? string.Empty));

            Claim role;
            if (token.GrantType == KoreAuthToken.BrandPublic)
            {
                role = new Claim(ClaimTypes.Role, "public");
            }
            else
            {
                role = new Claim(ClaimTypes.Role, "player");
            }
            claims.Add(role);

            // remove any existing claims
            foreach (Claim claim in claims)
            {
                if (identity.HasClaim(cl => cl.Type == claim.Type))
                {
                    Claim removeClaim = identity.FindFirst(cl => cl.Type == claim.Type);
                    identity.RemoveClaim(removeClaim);
                }
            }

            identity.AddClaims(claims);
        }

        public async Task<KoreAuthToken> SetKoreClaimsForPublicBrandAsync(string returnUrl, ClaimsPrincipal user)
        {
            var token = await _koreAuthService.GetPublicBrandTokenAsync(returnUrl, _koreOptions.Brand, _koreOptions.Locale);

            AddTokenToIdentityClaims(token, user);

            return token;
        }

        public async Task<KoreAuthToken> SetKoreClaimsForPlayerAsync(string returnUrl, string email, string password, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}
