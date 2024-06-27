using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.DashboardReports.Dto
{
    public class ReportDto
    {
        public int UsersCount { get; set; }
        public int AllTasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public int ToDoTasksCount { get; set; }
        public int InProgressTasksCount { get; set; }
        public int MyTasksCount { get; set; }
        public int MyTasksTeamCount { get; set; }
        public int TeamsCount { get; set; }
    }
    public class TopFiveUsersDto
    {
        public string UserName { get; set; }
        public int HisTasksCount { get; set; }
    }
}
