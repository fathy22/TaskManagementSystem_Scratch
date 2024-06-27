using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.DashboardReports.Dto;

namespace Application.DashboardReports
{
    public interface IDashboardReportAppService 
    {
        ReportDto GetDashboardCards();
        Task<List<TopFiveUsersDto>> GetTopFiveUsersHaveTasks();
    }
}
