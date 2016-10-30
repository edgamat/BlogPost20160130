using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crossroads.Server.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crossroads.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
     }
}
