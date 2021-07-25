using DotNetCoreCalendar.Data;
using DotNetCoreCalendar.Helpers;
using DotNetCoreCalendar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDAL _idal;
        private readonly UserManager<ApplicationUser> _usermanager;

        public HomeController(ILogger<HomeController> logger, IDAL idal, UserManager<ApplicationUser> usermanager)
        {
            _logger = logger;
            _idal = idal;
            _usermanager = usermanager;
        }

        public IActionResult Index()
        {
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetLocations());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetEvents());
            return View();
        }

        [Authorize]
        public IActionResult MyCalendar()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_idal.GetLocations());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_idal.GetMyEvents(userid));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
