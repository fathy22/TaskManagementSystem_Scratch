using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.CustomLogs;

namespace TaskManagementSystem.CustomLogs.Dto
{
    [AutoMapFrom(typeof(CustomLog))]
    public class CreateCustomLogDto
    {
        public string Description { get; set; }
    }
}
