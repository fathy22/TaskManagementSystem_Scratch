
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Teams.Dto;

namespace Application.Teams
{
    public interface ITeamAppService
    {
        Task<List<TeamDto>> GetAllTeams();
        Task<TeamDto> GetTeamById(int id);
        Task AddTeam(CreateTeamDto Team);
        Task UpdateTeam(TeamDto Team);
        Task DeleteTeam(int id);
        List<TeamMemberDto> GetTeamMembersByTeamId(int id);
    }
}
