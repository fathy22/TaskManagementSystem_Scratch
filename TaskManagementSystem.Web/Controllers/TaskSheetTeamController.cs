using Application.TaskSheets;
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
using TaskManagementSystem.Core.Sessions;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.TaskSheets.Dto;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize(Roles =StaticRoleNames.Host.TeamLeads)]
    public class TaskSheetTeamController : Controller
    {
        private readonly ITaskSheetAppService _taskSheetService;
        private readonly IUserAppService _userService;
        private readonly ITeamAppService _teamService;
        private readonly ICustomSession _customSession;

        public TaskSheetTeamController(ITaskSheetAppService TaskSheetService, IUserAppService userService, ITeamAppService teamService, ICustomSession customSession)
        {
            _taskSheetService = TaskSheetService;
            _userService = userService;
            _teamService = teamService;
            _customSession = customSession;
        }

        public async Task<IActionResult> Index()
        {
            var team = await _teamService.GetTeamMembersByByTeamLeaderId(_customSession.UserId);
            var TaskSheets = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto { TeamId = team.TeamId});
            return View(TaskSheets);
        }
        public async Task<IActionResult> Create()
        {
            var team = await _teamService.GetTeamMembersByByTeamLeaderId(_customSession.UserId);
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            ViewBag.Users = team.TeamMembers;
            ViewBag.Tasks = tasks;
            return View(new CreateTaskSheetDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskSheetDto model)
        {
            var team = await _teamService.GetTeamMembersByByTeamLeaderId(_customSession.UserId);
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            ViewBag.Users = team.TeamMembers;
            ViewBag.Tasks = tasks;
            if (ModelState.IsValid)
            {
                var TaskSheet = new CreateTaskSheetDto
                {
                    Title = model.Title,
                    AttachmentId = model.AttachmentId,
                    DependentTaskId = model.DependentTaskId,
                    Description=model.Description,
                    DueDate=model.DueDate,
                    IsDependentOnAnotherTask= model.IsDependentOnAnotherTask,
                    TaskPriority=model.TaskPriority,
                    TaskStatus=model.TaskStatus,
                    TeamId= team.TeamId,
                    UserId = model.UserId
                };
                await _taskSheetService.AddTaskSheet(TaskSheet);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var TaskSheet = await _taskSheetService.GetTaskSheetById(id);
            if (TaskSheet == null)
            {
                return NotFound();
            }

            var team = await _teamService.GetTeamMembersByByTeamLeaderId(_customSession.UserId);
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            tasks.RemoveAll(c => c.Id == id);
            ViewBag.Users = team.TeamMembers;
            ViewBag.Tasks = tasks;

            var model = new TaskSheetDto
            {
                Id = TaskSheet.Id,
                Title = TaskSheet.Title,
                AttachmentId = TaskSheet.AttachmentId,
                AttachmentName = TaskSheet.AttachmentName,
                DependentTaskId = TaskSheet.DependentTaskId,
                Description = TaskSheet.Description,
                DueDate = TaskSheet.DueDate,
                IsDependentOnAnotherTask = TaskSheet.IsDependentOnAnotherTask,
                TaskPriority = TaskSheet.TaskPriority,
                TaskStatus = TaskSheet.TaskStatus,
                TeamId = team.TeamId,
                UserId = TaskSheet.UserId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskSheetDto model)
        {
            var team = await _teamService.GetTeamMembersByByTeamLeaderId(_customSession.UserId);
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            ViewBag.Users = team.TeamMembers;
            ViewBag.Tasks = tasks;
            tasks.RemoveAll(c => c.Id == model.Id);
            if (ModelState.IsValid)
            {
                var taskSheet = await _taskSheetService.GetTaskSheetById(model.Id);
                if (taskSheet == null)
                {
                    return NotFound();
                }

                taskSheet.Title = model.Title;
                taskSheet.Description = model.Description;
                taskSheet.TaskStatus = model.TaskStatus;
                taskSheet.TaskPriority = model.TaskPriority;
                taskSheet.AttachmentId = model.AttachmentId;
                taskSheet.DependentTaskId = model.DependentTaskId;
                taskSheet.DueDate = model.DueDate;
                taskSheet.TeamId = team.TeamId;
                taskSheet.UserId = model.UserId;
                taskSheet.IsDependentOnAnotherTask = model.IsDependentOnAnotherTask;

                 await _taskSheetService.UpdateTaskSheet(taskSheet);
                 return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var TaskSheet = await _taskSheetService.GetTaskSheetById(id);
            if (TaskSheet == null)
            {
                return NotFound();
            }
            return View(TaskSheet);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             await _taskSheetService.DeleteTaskSheet(id);
             return RedirectToAction(nameof(Index));
        }
    }
}
