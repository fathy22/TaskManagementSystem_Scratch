﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Teams.Dto;
using TaskManagementSystem.Teams;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.CustomLogs.Dto;
using TaskManagementSystem.CustomLogs;
using TaskManagementSystem.TaskSheets.Dto;
using TaskManagementSystem.TaskComments.Dto;
using Application.TaskComments.Dto;

namespace TaskManagementSystem.Mapping
{
    public class TaskManagementSystemMappingProfile : Profile
    {
        public TaskManagementSystemMappingProfile()
        {
            //Team
            CreateMap<CreateTeamDto, Team>();
            CreateMap<TeamDto, Team>();

            //TeamMember
            CreateMap<CreateTeamMemberDto, TeamMember>();

            //Task
            CreateMap<CreateTaskSheetDto, TaskSheet>();
            CreateMap<TaskSheetDto, TaskSheet>();

            //CustomLog
            CreateMap<CreateCustomLogDto, CustomLog>();
            CreateMap<CustomLogDto, CustomLog>();


            //TaskComment
            CreateMap<CreateTaskCommentDto, TaskComment>();
            CreateMap<TaskCommentDto, TaskComment>();
        }
    }
}
