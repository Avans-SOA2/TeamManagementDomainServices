using Core.Domain;
using System.Collections.Generic;

namespace Core.DomainServices.Repos.Intf;

public interface IOpponentRepository
{
    IEnumerable<Opponent> GetOpponents();

    Opponent GetById(int id);
}