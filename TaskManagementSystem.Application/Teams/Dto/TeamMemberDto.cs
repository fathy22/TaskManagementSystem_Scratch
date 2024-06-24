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
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Helpers.Enums;
using TaskManagementSystem.Teams;

namespace TaskManagementSystem.Teams.Dto
{
    [AutoMapFrom(typeof(TeamMember))]
    public class TeamMemberDto
    {
        public string MemberId { get; set; }
        public string MemberName { get; set; }
    }
}
