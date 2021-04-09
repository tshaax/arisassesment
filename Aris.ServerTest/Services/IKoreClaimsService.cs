using System.Security.Claims;
using System.Threading.Tasks;
using Aris.ServerTest.Models;

namespace Aris.ServerTest.Services
{
    public interface IKoreClaimsService
    {
        void AddTokenToIdentityClaims(KoreAuthToken token, ClaimsPrincipal user);

        Task<KoreAuthToken> SetKoreClaimsForPublicBrandAsync(string returnUrl, ClaimsPrincipal user);

        Task<KoreAuthToken> SetKoreClaimsForPlayerAsync(string returnUrl, string email, string password, ClaimsPrincipal user);
    }
}
