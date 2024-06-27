using Abp.Domain.Repositories;
using Application.CustomLogs;
using Application.UnitOfWorks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Core.Sessions;
using TaskManagementSystem.Teams;
using TaskManagementSystem.Teams.Dto;

namespace Application.Teams
{
    public class TeamAppService : ITeamAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomSession _customSession;
        private readonly ICustomLogAppService _customLogAppService;

        public TeamAppService(IUnitOfWork unitOfWork, IMapper mapper, ICustomLogAppService customLogAppService, ICustomSession customSession)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customLogAppService = customLogAppService;
            _customSession = customSession;
        }
        public async Task<List<TeamDto>> GetAllTeams()
        {
            var teams = await _unitOfWork.GetRepository<Team>().GetAll(query =>
                query.Include(t => t.TeamLeader)
                     .Include(t => t.TeamMembers)
                     .ThenInclude(c => c.Member));

            return _mapper.Map<List<TeamDto>>(teams);
        }

        public async Task<TeamDto> GetTeamById(int id)
        {
            var teams = await _unitOfWork.GetRepository<Team>().GetAll(query =>
                query.Include(t => t.TeamLeader)
                     .Include(t => t.TeamMembers)
                     .ThenInclude(c => c.Member));
            var team = teams.FirstOrDefault(c=>c.Id==id);
            return _mapper.Map<TeamDto>(team);

        }

        public async Task AddTeam(CreateTeamDto team)
        {
            try
            {
                var newTeam = _mapper.Map<Team>(team);
                var addedTeam = await _unitOfWork.GetRepositoryAndSave(newTeam);
                if (team.SelectedMembers!=null)
                {
                    foreach (var member in team.SelectedMembers)
                    {
                        await AddTeamMemebr(new CreateTeamMemberDto
                        {
                            MemberId=member,
                            TeamId = addedTeam.Id
                        });
                    }
                }
                if (_customSession.UserId != null)
                {
                    var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                    await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                    {
                        Description = $"{user.FirstName} {user.SecondName} add new team"
                    });
                }
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private async Task AddTeamMemebr(CreateTeamMemberDto member)
        {
            try
            {
                var aut = _mapper.Map<TeamMember>(member);
                await _unitOfWork.GetRepository<TeamMember>().Add(aut);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private async Task RemoveTeamMemebr(int teamId)
        {
            try
            {
                var teams = await _unitOfWork.GetRepository<Team>().GetAll(query =>
                query.Include(t => t.TeamMembers));
                var team = teams.FirstOrDefault(c => c.Id == teamId);
                if (team.TeamMembers!=null && team.TeamMembers.Count>0)
                {
                    foreach (var memeber in team.TeamMembers)
                    {
                        await _unitOfWork.GetRepository<TeamMember>().Delete(memeber);
                        _unitOfWork.Save();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task UpdateTeam(TeamDto team)
        {
            try
            {
                var existingTeam = await _unitOfWork.GetRepository<Team>().GetById(team.Id);

                if (existingTeam == null)
                {
                    return;
                }
                _mapper.Map(team, existingTeam);

                await _unitOfWork.GetRepository<Team>().Update(existingTeam);
                await RemoveTeamMemebr(team.Id);
                if (team.SelectedMembers != null)
                {
                    foreach (var member in team.SelectedMembers)
                    {
                        await AddTeamMemebr(new CreateTeamMemberDto
                        {
                            MemberId = member,
                            TeamId = existingTeam.Id
                        });
                    }
                }
                if (_customSession.UserId != null)
                {
                    var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                    await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                    {
                        Description = $"{user.FirstName} {user.SecondName} update task"
                    });
                }
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task DeleteTeam(int id)
        {
            var existingTeam = await _unitOfWork.GetRepository<Team>().GetById(id);

            if (existingTeam == null)
            {
                return;
            }
            await _unitOfWork.GetRepository<Team>().Delete(existingTeam);
            if (_customSession.UserId != null)
            {
                var user = await _customLogAppService.GetCurrentUserName(_customSession.UserId);
                await _customLogAppService.AddCustomLog(new TaskManagementSystem.CustomLogs.Dto.CreateCustomLogDto
                {
                    Description = $"{user.FirstName} {user.SecondName} delete team"
                });
            }
            _unitOfWork.Save();

        }
        public List<TeamMemberDto> GetTeamMembersByTeamId(int id)
        {
            var members = _unitOfWork.GetRepository<TeamMember>().GetAll().Result.Where(v=>v.TeamId == id).ToList();
            return _mapper.Map<List<TeamMemberDto>>(members);
        }
        public async Task<TeamShowDto> GetTeamMembersByByTeamLeaderId(string teamLeaderId)
        {
            try
            {
                var data = new TeamShowDto();
                var teams = await _unitOfWork.GetRepository<Team>().GetAll(query =>
                query.Include(t => t.TeamLeader)
                     .Include(t => t.TeamMembers)
                     .ThenInclude(c => c.Member));
                var team = teams.FirstOrDefault(c => c.TeamLeaderId == teamLeaderId);

                data.TeamMembers = team.TeamMembers.Select(u => new ApplicationUser
                {
                    Id = u.MemberId,
                    FirstName = u.Member.FirstName + u.Member.SecondName
                }).ToList();
                data.TeamId = team.Id;
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

    }
}
