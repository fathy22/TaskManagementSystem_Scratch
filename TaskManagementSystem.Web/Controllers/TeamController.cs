using Application.Teams;
using Application.Users;
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
    public class TeamController : Controller
    {
        private readonly ITeamAppService _teamService;
        private readonly IUserAppService _userService;

        public TeamController(ITeamAppService teamService, IUserAppService userService)
        {
            _teamService = teamService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _teamService.GetAllTeams();
            return View(teams);
        }
        public async Task<IActionResult> Create()
        {
            var teamLeaders = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.TeamLeads);
            var regularUsers = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.RegularUsers);
            ViewBag.TeamLeaders = teamLeaders;
            ViewBag.RegularUsers = regularUsers;
            return View(new TeamDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamDto model)
        {
            if (ModelState.IsValid)
            {
                var team = new CreateTeamDto
                {
                    Name = model.Name,
                    TeamLeaderId = model.TeamLeaderId,
                    TeamMembers = model.TeamMembers
                };
                await _teamService.AddTeam(team);
                return RedirectToAction(nameof(Index));
            }

            var teamLeaders = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.TeamLeads);
            var regularUsers = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.RegularUsers);
            ViewBag.TeamLeaders = teamLeaders;
            ViewBag.RegularUsers = regularUsers;
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetTeamById(id);
            if (team == null)
            {
                return NotFound();
            }

            var teamLeaders = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.TeamLeads);
            var regularUsers = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.RegularUsers);
            ViewBag.TeamLeaders = teamLeaders;
            ViewBag.RegularUsers = regularUsers;

            var selectedmembers = _teamService.GetTeamMembersByTeamId(id);

            var model = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                TeamLeaderId = team.TeamLeaderId,
                TeamMembers = selectedmembers.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTeamDto model)
        {
            if (ModelState.IsValid)
            {
                var team = await _teamService.GetTeamById(model.Id);
                if (team == null)
                {
                    return NotFound();
                }

                team.Name = model.Name;
                team.TeamLeaderId = model.TeamLeaderId;
                team.TeamMembers = model.TeamMembers;

                 await _teamService.UpdateTeam(team);
                 return RedirectToAction(nameof(Index));
            }

            var teamLeaders = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.TeamLeads);
            var regularUsers = await _userService.GetUsersByRoleAsync(StaticRoleNames.Host.RegularUsers);
            ViewBag.TeamLeaders = teamLeaders;
            ViewBag.RegularUsers = regularUsers;
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var team = await _teamService.GetTeamById(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             await _teamService.DeleteTeam(id);
             return RedirectToAction(nameof(Index));
        }
    }
}
