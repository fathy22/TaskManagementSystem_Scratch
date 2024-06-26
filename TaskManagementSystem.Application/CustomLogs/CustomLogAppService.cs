﻿using Application.UnitOfWorks;
using Application.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.CustomLogs;
using TaskManagementSystem.CustomLogs.Dto;

namespace Application.CustomLogs
{
    public class CustomLogAppService : ICustomLogAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomLogAppService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<CustomLogDto>> GetAllCustomLogs()
        {
            var CustomLogs = _unitOfWork.GetRepository<CustomLog>().GetAll().Result.OrderByDescending(c=>c.Id).ToList();

            return _mapper.Map<List<CustomLogDto>>(CustomLogs);
        }

        public async Task AddCustomLog(CreateCustomLogDto CustomLog)
        {
            try
            {
                var aut = _mapper.Map<CustomLog>(CustomLog);
                await _unitOfWork.GetRepository<CustomLog>().Add(aut);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<ApplicationUser> GetCurrentUserName(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
    }
}
