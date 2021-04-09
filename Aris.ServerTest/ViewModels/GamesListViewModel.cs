using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace Aris.ServerTest.ViewModels
{
    public class GamesListViewModel
    {

        public IEnumerable<Models.KoreGame> Games { get; set; }

    }
}
