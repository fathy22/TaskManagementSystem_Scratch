using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;

namespace TaskManagementSystem.Teams
{
    public class TeamMember:FullAuditedEntity<int>
    {
        public Team Team { get; set; }
        public int TeamId { get; set; }

        public string MemberId { get; set; }

        [ForeignKey("MemberId")]
        public ApplicationUser Member { get; set; }

    }
}
