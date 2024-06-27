using Application.CustomLogs;
using Application.TaskComments.Dto;
using Application.UnitOfWorks;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Sessions;
using TaskManagementSystem.TaskComments;
using TaskManagementSystem.TaskComments.Dto;
using TaskManagementSystem.Tasks;

namespace Application.TaskComments
{
    public class TaskCommentAppService : ITaskCommentAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomSession _customSession;
        private readonly ICustomLogAppService _customLogAppService;

        public TaskCommentAppService(IUnitOfWork unitOfWork, IMapper mapper, ICustomSession customSession, ICustomLogAppService customLogAppService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customSession = customSession;
            _customLogAppService = customLogAppService;
        }
        public async Task<List<TaskCommentDto>> GetAllTaskComments()
        {
            var TaskComments = await _unitOfWork.GetRepository<TaskComment>().GetAll();
            return _mapper.Map<List<TaskCommentDto>>(TaskComments);
        }

        public async Task AddTaskComment(CreateTaskCommentDto TaskComment)
        {
            try
            {
                var aut = _mapper.Map<TaskComment>(TaskComment);
                await _unitOfWork.GetRepository<TaskComment>().Add(aut);
                if (_customSession.UserId != null)
                {
                    var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                    await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                    {
                        Description = $"{user.FirstName} {user.SecondName} add new comment"
                    });
                }
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
