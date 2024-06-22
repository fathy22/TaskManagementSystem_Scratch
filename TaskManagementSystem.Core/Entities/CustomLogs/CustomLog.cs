using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.CustomLogs
{
    public class CustomLog : FullAuditedEntity<int>
    {
        public string Description { get; set; }
    }
}
