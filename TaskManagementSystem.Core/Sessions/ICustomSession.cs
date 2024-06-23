using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Sessions
{
    public interface ICustomSession
    {
        string UserId { get; set; }
        string RoleId { get; set; }
    }
}
