using System.Collections.Generic;
using Core.Domain;

namespace Core.DomainServices.Repos.Intf;

public interface ITeamRepository
{
    IEnumerable<Team> GetTeams();
}