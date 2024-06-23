
using Application.TaskComments.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.TaskComments.Dto;

namespace Application.TaskComments
{
    public interface ITaskCommentAppService
    {
        Task<List<TaskCommentDto>> GetAllTaskComments();
        Task AddTaskComment(CreateTaskCommentDto TaskComment);
    }
}
