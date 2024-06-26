﻿using Abp.AutoMapper;
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
    [AutoMapFrom(typeof(TeamMember))]
    public class CreateTeamMemberDto
    {
        public string MemberId { get; set; }
        public int TeamId { get; set; }
    }
}
