﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aris.ServerTest.Filters;

namespace Aris.ServerTest.Controllers
{
    
    [ReturnUrlFromRequest]
    public class HomeController : BaseController
    {
        private readonly Services.IKoreApiGameService _gameService;

        public HomeController(Services.IKoreApiGameService gameService) 
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index(string catFilter, string returnUrl)
        {
            var viewModel = new ViewModels.GamesListViewModel();
            var games = await _gameService.GetGamesAsync(GetAuthToken(), returnUrl);

            viewModel.Games = games.OrderByDescending(o=> o.Category).ThenBy(o => o.Platform).ThenBy(o => o.Name);

            ViewBag.Category = viewModel.Games.AsEnumerable().Select(li => new SelectListItem
            {
                Text = li.Category,
                Value = li.Category,
                Selected = li.Category == catFilter
            });

            return View(viewModel);
        }

        public async Task<IActionResult> Details(string game, string returnUrl)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(game);
            var gameUrl = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            var gameJson = await _gameService.GetGameAsync(GetAuthToken(), gameUrl, returnUrl);

            return Json(gameJson);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}