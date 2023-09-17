using System.Collections.Generic;
using Core.Domain;

namespace Core.DomainServices.Repos.Intf;

public interface ICoachRepository
{
    IEnumerable<Coach> GetCoaches();
    Coach GetById(int id);
}