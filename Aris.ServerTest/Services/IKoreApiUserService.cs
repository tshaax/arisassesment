using System.Threading.Tasks;
using Aris.ServerTest.Models;

namespace Aris.ServerTest.Services
{
    public interface IKoreApiUserService
    {
        Task<KoreUserLinks> GetUserLinksAsync(KoreAuthToken token, string returnUrl);
    }
}