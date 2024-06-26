
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.TaskSheets.Dto;

namespace Application.TaskSheets
{
    public interface ITaskSheetAppService
    {
        Task<List<TaskSheetDto>> GetAllTaskSheets(TaskSheetFilterDto input);
        Task<TaskSheetDto> GetTaskSheetById(int id);
        Task AddTaskSheet(CreateTaskSheetDto TaskSheet);
        Task UpdateTaskSheet(TaskSheetDto TaskSheet);
        Task DeleteTaskSheet(int id);
    }
}
