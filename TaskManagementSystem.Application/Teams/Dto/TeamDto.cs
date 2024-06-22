using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Attachments;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Teams;

namespace TaskManagementSystem.Teams.Dto
{
    [AutoMapFrom(typeof(Team))]
    public class TeamDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long TeamLeaderId { get; set; }
        public List<CreateTeamMemberDto> TeamMembers { get; set; }
    }
    public class TeamShowDto
    {
        public int TeamId { get; set; }
    }
}
