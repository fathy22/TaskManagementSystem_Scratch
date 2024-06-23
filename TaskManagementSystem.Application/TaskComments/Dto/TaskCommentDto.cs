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
using TaskManagementSystem.TaskComments;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.TaskSheets.Dto;

namespace TaskManagementSystem.TaskComments.Dto
{
    [AutoMapFrom(typeof(TaskComment))]
    public class TaskCommentDto
    {
        public int TaskSheetId { get; set; }
        public TaskSheetDto TaskSheet { get; set; }
        public string Comment { get; set; }
    }
}
