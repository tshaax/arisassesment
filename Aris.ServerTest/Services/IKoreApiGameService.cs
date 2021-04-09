using System.Collections.Generic;
using System.Threading.Tasks;
using Aris.ServerTest.Models;

namespace Aris.ServerTest.Services
{
    public interface IKoreApiGameService
    {
        Task<List<KoreGame>> GetGamesAsync(KoreAuthToken token, string returnUrl);

        Task<KoreGame> GetGameAsync(KoreAuthToken token, string gameUrl, string returnUrl);
        
    }
}