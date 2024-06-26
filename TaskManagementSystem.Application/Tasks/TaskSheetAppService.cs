using Abp.Collections.Extensions;
using Application.CustomLogs;
using Application.UnitOfWorks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Sessions;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.TaskSheets;
using TaskManagementSystem.TaskSheets.Dto;
using TaskManagementSystem.Teams;
using TaskManagementSystem.Teams.Dto;

namespace Application.TaskSheets
{
    public class TaskSheetAppService : ITaskSheetAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomSession _customSession;
        private readonly ICustomLogAppService _customLogAppService;
        public TaskSheetAppService(IUnitOfWork unitOfWork, IMapper mapper, ICustomSession customSession, ICustomLogAppService customLogAppService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customSession = customSession;
            _customLogAppService = customLogAppService;
        }
        public async Task<List<TaskSheetDto>> GetAllTaskSheets(TaskSheetFilterDto input)
        {
            try
            {
                var TaskSheets = await _unitOfWork.GetRepository<TaskSheet>().GetAll(query =>
                query.Include(t => t.Attachment).Include(t=>t.Team));
                var list  =  _mapper.Map<List<TaskSheetDto>>(TaskSheets);
                list = list
                    .WhereIf(!string.IsNullOrEmpty(input.UserId), at => at.UserId == input.UserId)
                    .WhereIf(input.TeamId.HasValue, at => at.TeamId == input.TeamId).ToList();
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public async Task<TaskSheetDto> GetTaskSheetById(int id)
        {
            var teams = await _unitOfWork.GetRepository<TaskSheet>().GetAll(query =>
               query.Include(t => t.Attachment));
            var task = teams.FirstOrDefault(c => c.Id == id);
            return _mapper.Map<TaskSheetDto>(task);

        }

        public async Task AddTaskSheet(CreateTaskSheetDto TaskSheet)
        {
            try
            {
                var aut = _mapper.Map<TaskSheet>(TaskSheet);
                await _unitOfWork.GetRepository<TaskSheet>().Add(aut);
                var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                {
                    Description = $"{user.FirstName} {user.SecondName} add new task"
                });
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task UpdateTaskSheet(TaskSheetDto TaskSheet)
        {
            try
            {
                var existingTaskSheet = await _unitOfWork.GetRepository<TaskSheet>().GetById(TaskSheet.Id);

                if (existingTaskSheet == null)
                {
                    return;
                }
                _mapper.Map(TaskSheet, existingTaskSheet);

                await _unitOfWork.GetRepository<TaskSheet>().Update(existingTaskSheet);
                var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                {
                    Description = $"{user.FirstName} {user.SecondName} update task"
                });
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task DeleteTaskSheet(int id)
        {
            var existingTaskSheet = await _unitOfWork.GetRepository<TaskSheet>().GetById(id);

            if (existingTaskSheet == null)
            {
                return;
            }
            var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
            await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
            {
                Description = $"{user.FirstName} {user.SecondName} delete task"
            });
            await _unitOfWork.GetRepository<TaskSheet>().Delete(existingTaskSheet);
            _unitOfWork.Save();

        }
    }
}
