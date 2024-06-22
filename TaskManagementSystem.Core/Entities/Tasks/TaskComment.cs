using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.Teams;

namespace TaskManagementSystem.Tasks
{
    public class TaskComment : FullAuditedEntity<int>
    {
        public int TaskSheetId { get; set; }
        [ForeignKey("TaskSheetId")]
        public TaskSheet TaskSheet { get; set; }
        public string Comment { get; set; }
    }
}
