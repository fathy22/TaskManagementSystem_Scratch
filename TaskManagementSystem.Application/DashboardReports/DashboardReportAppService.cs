using Abp.Application.Services;
using Abp.Domain.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.DashboardReports.Dto;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.Teams;

namespace Application.DashboardReports
{
    public class DashboardReportAppService : ApplicationService, IDashboardReportAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardReportAppService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public ReportDto GetDashboardCards()
        {
            var data = new ReportDto();
            data.UsersCount = _userManager.Users.ToList().Count();
            data.TeamsCount = _unitOfWork.GetRepository<Team>().GetAll().Result.Count();
            data.AllTasksCount = _unitOfWork.GetRepository<TaskSheet>().GetAll().Result.Count();
            data.CompletedTasksCount = _unitOfWork.GetRepository<TaskSheet>().GetAll().Result.Where(c =>c.TaskStatus ==TaskSheetStatus.Completed).Count();
            data.ToDoTasksCount = _unitOfWork.GetRepository<TaskSheet>().GetAll().Result.Where(c => c.TaskStatus == TaskSheetStatus.ToDo).Count();
            data.InProgressTasksCount = _unitOfWork.GetRepository<TaskSheet>().GetAll().Result.Where(c => c.TaskStatus == TaskSheetStatus.InProgress).Count();
            return data;
        }

        public async Task<List<TopFiveUsersDto>> GetTopFiveUsersHaveTasks()
        {
            var data = new List<TopFiveUsersDto>();

            var tasks =await _unitOfWork.GetRepository<TaskSheet>().GetAll();
            var count = tasks.GroupBy(t => t.UserId).Select(v => new TopFiveUsersDto
            {
                UserName = v.FirstOrDefault()?.User?.FirstName + " " + v.FirstOrDefault()?.User?.SecondName,
                HisTasksCount =v.Count(),
            }).ToList();
            var list = count.OrderByDescending(v => v.HisTasksCount).Take(5).ToList();
            return list;
        }
    }
}
