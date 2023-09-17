using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Infrastructure.TMEF;

public class GameDbContext : DbContext
{
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Opponent> Opponents { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<CareTaker> CareTakers { get; set; }

    public GameDbContext(DbContextOptions<GameDbContext> contextOptions) : base(contextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        IEnumerable<Coach> coaches = new List<Coach> {
            new Coach { Id = 1, Name = "Louis van Gaal" },
            new Coach { Id = 2, Name = "Ronald Koeman" }};

        IEnumerable<Team> teams = new List<Team> {
            new Team { Id = 1, Name = "Chicago Bulls senioren", TeamHeadCoachId = coaches.ToList()[0].Id, Players = new List<Player>()},
            new Team { Id = 2, Name = "Chicago Bulls junioren", TeamHeadCoachId = coaches.ToList()[1].Id, Players = new List<Player>() }};

        IEnumerable<Address> playingAddresses = new List<Address> {
            new Address { AddressId = 1, City = "Los Angeles", Street = "S. Figueroa St.", Number = 1111 },
            new Address { AddressId = 2, City = "Indianapolis", Street = "South Pennsylvania Street", Number = 125 }};

        IEnumerable<Opponent> opponents = new List<Opponent> {
            new Opponent { Id = 1, Name = "LA Lakers", PlayingAddressId = 1 },
            new Opponent { Id = 2, Name = "Indiana Pacers" , PlayingAddressId = 2 } };

        IEnumerable<Player> players = new List<Player> {
            new Player { Id = 1, Name = "Michael Jordan", PlayerNumber = 23, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 2, Name = "Scotty Pippen", PlayerNumber = 33, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 3, Name = "Lebron James", PlayerNumber = 1, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 4, Name = "Johan Smarius", PlayerNumber = 2, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 5, Name = "Marcel de Groot", PlayerNumber = 3, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 6, Name = "Pascal van Gastel", PlayerNumber = 4, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 7, Name = "Erco Argante", PlayerNumber = 5, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 8, Name = "Ruud Hermans", PlayerNumber = 6, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 9, Name = "Arno Broeders", PlayerNumber = 7, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 10, Name = "Eefje Gijzen", PlayerNumber = 8, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 11, Name = "Dion Koeze", PlayerNumber = 9, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 12, Name = "Peter Gerards", PlayerNumber = 10, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 13, Name = "Ger Oosting", PlayerNumber = 11, CareTakers = new List<CareTaker>(), TeamId = teams.First().Id },
            new Player { Id = 14, Name = "Robin Schellius", PlayerNumber = 12, CareTakers = new List<CareTaker>(), TeamId = teams.ToArray()[1].Id }};

        IEnumerable<CareTaker> caretakers = new List<CareTaker> {
            new CareTaker { Id = 1, Name = "F. Jacobse", PlayerId = 1, HasCar = true},
            new CareTaker { Id = 2, Name = "Ted van Es", PlayerId = 2},
            new CareTaker { Id = 3, Name = "Hans van der Vaart", PlayerId = 3, HasCar = true},
            new CareTaker { Id = 4, Name = "Tjolk Hekking", PlayerId = 4},
            new CareTaker { Id = 5, Name = "Remco Clavan", PlayerId = 5, HasCar = true},
            new CareTaker { Id = 6, Name = "Otto den Beste", PlayerId = 6},
            new CareTaker { Id = 7, Name = "Edgar", PlayerId = 7, HasCar = true},
            new CareTaker { Id = 8, Name = "Jos", PlayerId = 8},
            new CareTaker { Id = 9, Name = "Storm", PlayerId = 9, HasCar = true}};

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Address>().HasData(playingAddresses);

        modelBuilder.Entity<Coach>().HasData(coaches);

        modelBuilder.Entity<Opponent>().HasData(opponents);

        modelBuilder.Entity<Team>().HasData(teams);

        modelBuilder.Entity<CareTaker>().HasData(caretakers);

        modelBuilder.Entity<Player>().HasData(players);
    }
}