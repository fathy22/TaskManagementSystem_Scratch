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
    [AutoMapFrom(typeof(Team))]
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string TeamLeaderId { get; set; }
        public List<string> SelectedMembers { get; set; } = new List<string>();
    }
}
