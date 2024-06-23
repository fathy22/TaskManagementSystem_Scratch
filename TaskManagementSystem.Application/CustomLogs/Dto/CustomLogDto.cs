using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.CustomLogs;
using TaskManagementSystem.Helpers.Enums;

namespace TaskManagementSystem.CustomLogs.Dto
{
    [AutoMapFrom(typeof(CustomLog))]
    public class CustomLogDto
    {
        public string Description { get; set; }
    }
}
