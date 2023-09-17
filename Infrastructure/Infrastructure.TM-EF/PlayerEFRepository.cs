using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices.Repos.Intf;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TMEF;

public class PlayerEFRepository : IPlayerRepository
{
    private readonly GameDbContext _context;

    public PlayerEFRepository(GameDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Player> GetPlayers()
    {
        return _context.Players.Include(p => p.CareTakers).Include(p => p.Games);
    }

    public async Task AddPlayer(Player newPlayer)
    {
        _context.Players.Add(newPlayer);
        await _context.SaveChangesAsync();
    }
}