using System.Threading.Tasks;
using Aris.ServerTest.Models;

namespace Aris.ServerTest.Services
{
    public interface IKoreApiAuthService
    {
        Task<KoreAuthToken> GetPlayerTokenAsync(string returnUrl, string brand, string locale, string email, string password);
        Task<KoreAuthToken> GetPublicBrandTokenAsync(string returnUrl, string brand, string locale);
    }
}