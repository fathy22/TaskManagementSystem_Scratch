
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.CustomLogs.Dto;

namespace Application.CustomLogs
{
    public interface ICustomLogAppService
    {
        Task<List<CustomLogDto>> GetAllCustomLogs();
        Task AddCustomLog(CreateCustomLogDto CustomLog);
    }
}
