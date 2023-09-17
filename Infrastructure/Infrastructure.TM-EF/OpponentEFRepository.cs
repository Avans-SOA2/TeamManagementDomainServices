using Core.Domain;
using Core.DomainServices.Repos.Intf;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.TMEF;

public class OpponentEFRepository : IOpponentRepository
{
    private readonly GameDbContext _context;

    public OpponentEFRepository(GameDbContext context)
    {
        _context = context;
    }


    public IEnumerable<Opponent> GetOpponents()
    {
        return _context.Opponents.Include(o => o.PlayingAddress);
    }

    public Opponent GetById(int id)
    {
        return _context.Opponents.Include(o => o.PlayingAddress).SingleOrDefault(opponent => opponent.Id == id);
    }
}