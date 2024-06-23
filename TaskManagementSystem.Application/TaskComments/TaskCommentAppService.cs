using Application.TaskComments.Dto;
using Application.UnitOfWorks;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.TaskComments;
using TaskManagementSystem.TaskComments.Dto;
using TaskManagementSystem.Tasks;

namespace Application.TaskComments
{
    public class TaskCommentService : ITaskCommentAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskCommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
