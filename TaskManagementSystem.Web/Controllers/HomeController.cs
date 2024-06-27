using Application.CustomLogs;
using Application.DashboardReports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagementSystem.Core.Permissions;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    //[Authorize(Permission.ManageUsers)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardReportAppService _dashboardReportAppService;
        public HomeController(ILogger<HomeController> logger, IDashboardReportAppService dashboardReportAppService)
        {
            _logger = logger;
            _dashboardReportAppService = dashboardReportAppService;
        }

        public async Task<ActionResult> Index()
        {
            var counts = _dashboardReportAppService.GetDashboardCards();
            var userTasks = await _dashboardReportAppService.GetTopFiveUsersHaveTasks();
            var model = new DashboardViewModel
            {
                Report = counts,
                TopFiveUsers = userTasks
            };
            return View(model);
        }
        public IActionResult AccessDenied()
        {
            return View("~/Views/Home/AccessDenied.cshtml");
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
    }
}