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
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.TaskSheets;
using TaskManagementSystem.Teams;
using TaskManagementSystem.Teams.Dto;

namespace TaskManagementSystem.TaskSheets.Dto
{
    [AutoMapFrom(typeof(TaskSheet))]
    public class TaskSheetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskSheetStatus TaskStatus { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int? AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public int? TeamId { get; set; }
        public TeamDto Team { get; set; }
        public bool IsDependentOnAnotherTask { get; set; }
        public int? DependentTaskId { get; set; }
    }
    public class TaskSheetShowDto
    {
        public int TaskSheetId { get; set; }
    }
}
