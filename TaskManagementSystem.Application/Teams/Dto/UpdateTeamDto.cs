
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Teams;

namespace TaskManagementSystem.Teams.Dto
{
    [AutoMapFrom(typeof(Team))]
    public class UpdateTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long TeamLeaderId { get; set; }
        public List<int> TeamMembers { get; set; }
    }
}
