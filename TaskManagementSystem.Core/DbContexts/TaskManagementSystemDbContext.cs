
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.CustomLogs;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.Teams;

namespace DbContexts
{
    public class TaskManagementSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TaskSheet> TaskSheets { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<CustomLog> CustomLogs { get; set; }
        public TaskManagementSystemDbContext(DbContextOptions<TaskManagementSystemDbContext> options)
        : base(options)
        {
        }
    }
}
