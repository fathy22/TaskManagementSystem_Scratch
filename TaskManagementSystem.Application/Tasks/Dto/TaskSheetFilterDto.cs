
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.TaskSheets;

namespace TaskManagementSystem.TaskSheets.Dto
{
    public class TaskSheetFilterDto
    {
        public string? UserId { get; set; }
        public int? TeamId { get; set; }
    }
}
