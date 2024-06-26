using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Teams;

namespace TaskManagementSystem.Tasks
{
    public class TaskSheet:FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskSheetStatus TaskStatus { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public int? AttachmentId { get; set; }
        [ForeignKey("AttachmentId")]
        public Attachment? Attachment { get; set; }
        public int? TeamId { get; set; }
        [ForeignKey("TeamId")]
        public  Team? Team { get; set; }
        public  bool IsDependentOnAnotherTask { get; set; }

        public int? DependentTaskId { get; set; }
        [ForeignKey("DependentTaskId")]
        public TaskSheet? DependentTask { get; set; }
    }
}
