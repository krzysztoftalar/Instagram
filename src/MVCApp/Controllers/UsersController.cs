using DesktopUI.Library.Api.User;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;
using System;
using System.Threading.Tasks;

namespace MVCApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserEndpoint _userEndpoint;

        public UsersController(IUserEndpoint userEndpoint)
        {
            _userEndpoint = userEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string emailToken)
        {
            try
            {
                await _userEndpoint.VerifyEmail(userId, emailToken);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }

            return View();
        }
    }
}
