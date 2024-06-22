using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Attachments
{
    public class Attachment: FullAuditedEntity<int>
    {
        public virtual string Description { get; set; }
        public virtual string Name { get; set; }

        public virtual long Length { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string Extension { get; set; }
        public virtual string Uri { get; set; }
        public virtual string StoredFileName { get; set; }
    }
}
