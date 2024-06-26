using Application.CustomLogs;
using Application.Teams;
using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Authorization.Roles;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Teams.Dto;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomLogController : Controller
    {
        private readonly ICustomLogAppService _customLogService;

        public CustomLogController(ICustomLogAppService customLogService)
        {
            _customLogService = customLogService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _customLogService.GetAllCustomLogs();
            return View(logs);
        }
    }
}
