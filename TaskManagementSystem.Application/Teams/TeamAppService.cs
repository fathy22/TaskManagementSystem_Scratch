using Application.UnitOfWorks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Teams;
using TaskManagementSystem.Teams.Dto;

namespace Application.Teams
{
    public class TeamAppService : ITeamAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamAppService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task UpdateTeam(TeamDto Team)
        {
            try
            {
                var existingTeam = await _unitOfWork.GetRepository<Team>().GetById(Team.Id);

                if (existingTeam == null)
                {
                    return;
                }
                _mapper.Map(Team, existingTeam);

                await _unitOfWork.GetRepository<Team>().Update(existingTeam);

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
            _unitOfWork.Save();

        }
        public List<TeamMemberDto> GetTeamMembersByTeamId(int id)
        {
            var members = _unitOfWork.GetRepository<TeamMember>().GetAll().Result.Where(v=>v.TeamId == id).ToList();
            return _mapper.Map<List<TeamMemberDto>>(members);
        }
    }
}
