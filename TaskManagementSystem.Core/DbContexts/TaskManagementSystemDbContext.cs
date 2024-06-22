
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;

namespace DbContexts
{
    public class TaskManagementSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public TaskManagementSystemDbContext(DbContextOptions<TaskManagementSystemDbContext> options)
        : base(options)
        {
        }
    }
}
