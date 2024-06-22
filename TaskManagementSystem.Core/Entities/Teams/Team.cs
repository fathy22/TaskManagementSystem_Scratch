using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.Core.Entities;

namespace TaskManagementSystem.Teams
{
    public class Team : FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string TeamLeaderId { get; set; }
        [ForeignKey("TeamLeaderId")]
        public ApplicationUser TeamLeader { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}
