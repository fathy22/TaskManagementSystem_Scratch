﻿using Application.Teams;
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
            return View(new CreateTeamDto());
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
                    SelectedMembers = model.SelectedMembers
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

            var members = _teamService.GetTeamMembersByTeamId(id);
            var selectedmembers = members.Select(v => v.MemberId).ToList();

            var model = new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                TeamLeaderId = team.TeamLeaderId,
                SelectedMembers = selectedmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeamDto model)
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
                team.SelectedMembers = model.SelectedMembers;

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
