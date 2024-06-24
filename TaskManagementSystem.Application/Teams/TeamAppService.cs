using Application.UnitOfWorks;
using AutoMapper;
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
            var Teams = await _unitOfWork.GetRepository<Team>().GetAll();
            return _mapper.Map<List<TeamDto>>(Teams);
        }

        public async Task<TeamDto> GetTeamById(int id)
        {
            var Team = await _unitOfWork.GetRepository<Team>().GetById(id);
            return _mapper.Map<TeamDto>(Team);

        }

        public async Task AddTeam(CreateTeamDto Team)
        {
            try
            {
                var aut = _mapper.Map<Team>(Team);
                await _unitOfWork.GetRepository<Team>().Add(aut);
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
