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
    [Authorize(Roles = StaticRoleNames.Host.RegularUsers + "," + StaticRoleNames.Host.TeamLeads)]
    public class MyTaskSheetController : Controller
    {
        private readonly ITaskSheetAppService _taskSheetService;
        private readonly IUserAppService _userService;
        private readonly ITeamAppService _teamService;
        private readonly ICustomSession _customSession;

        public MyTaskSheetController(ITaskSheetAppService TaskSheetService, IUserAppService userService, ITeamAppService teamService, ICustomSession customSession)
        {
            _taskSheetService = TaskSheetService;
            _userService = userService;
            _teamService = teamService;
            _customSession = customSession;
        }

        public async Task<IActionResult> Index()
        {
            var TaskSheets = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto { UserId = _customSession.UserId});
            return View(TaskSheets);
        }
        public async Task<IActionResult> Create()
        {
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            ViewBag.Tasks = tasks;
            return View(new CreateTaskSheetDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskSheetDto model)
        {
            if (ModelState.IsValid)
            {
                var TaskSheet = new CreateTaskSheetDto
                {
                    Title = model.Title,
                    AttachmentId = model.AttachmentId,
                    DependentTaskId = model.DependentTaskId,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    IsDependentOnAnotherTask = model.IsDependentOnAnotherTask,
                    TaskPriority = model.TaskPriority,
                    TaskStatus = model.TaskStatus,
                    UserId = _customSession.UserId
                };
                await _taskSheetService.AddTaskSheet(TaskSheet);
                return RedirectToAction(nameof(Index));
            }
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            ViewBag.Tasks = tasks;
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var TaskSheet = await _taskSheetService.GetTaskSheetById(id);
            if (TaskSheet == null)
            {
                return NotFound();
            }

            var users = await _userService.GetAllUsersAsync();
            var teams = await _teamService.GetAllTeams();
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            tasks.RemoveAll(c => c.Id == id);
            ViewBag.Users = users;
            ViewBag.Teams = teams;
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
                UserId = _customSession.UserId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskSheetDto model)
        {
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
                taskSheet.UserId = _customSession.UserId;
                taskSheet.IsDependentOnAnotherTask = model.IsDependentOnAnotherTask;

                 await _taskSheetService.UpdateTaskSheet(taskSheet);
                 return RedirectToAction(nameof(Index));
            }
            var tasks = await _taskSheetService.GetAllTaskSheets(new TaskSheetFilterDto());
            ViewBag.Tasks = tasks;
            tasks.RemoveAll(c => c.Id == model.Id);
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
