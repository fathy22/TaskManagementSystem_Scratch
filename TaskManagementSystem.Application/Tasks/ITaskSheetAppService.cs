﻿
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
        Task<List<TaskSheetDto>> GetAllTaskSheets();
        Task<TaskSheetDto> GetTaskSheetById(int id);
        Task AddTaskSheet(CreateTaskSheetDto TaskSheet);
        Task UpdateTaskSheet(UpdateTaskSheetDto TaskSheet);
        Task DeleteTaskSheet(int id);
    }
}