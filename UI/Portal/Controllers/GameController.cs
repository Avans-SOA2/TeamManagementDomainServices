using Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models;
using Microsoft.EntityFrameworkCore;
using Core.DomainServices.Repos.Intf;
using Core.DomainServices.Services.Intf;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Portal.Controllers;

public class GameController : Controller
{
    // DomainServices
    private readonly IGameService _gameService;

    // Repositories
    private readonly ILogger<HomeController> _logger;
    private readonly IGameRepository _gameRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IOpponentRepository _opponentRepository;
    private readonly ITeamRepository _teamRepository;

    public GameController(
        ILogger<HomeController> logger,
        IGameRepository gameRepository,
        ICoachRepository coachRepository,
        IPlayerRepository playerRepository,
        IOpponentRepository opponentRepository,
        ITeamRepository teamRepository,
        IGameService gameService)
    {
        _logger = logger;
        _gameRepository = gameRepository;
        _coachRepository = coachRepository;
        _playerRepository = playerRepository;
        _teamRepository = teamRepository;
        _opponentRepository = opponentRepository ?? throw new ArgumentNullException(nameof(opponentRepository));
        _gameService = gameService;
    }

    public IActionResult Index()
    {
        return View(_gameRepository.GetAll().ToViewModel());
    }

    [Authorize]
    public IActionResult List(string team)
    {
        var model = _gameRepository.GetAll().Include(game => game.Team)
            .Where(game => team == null || game.Team.Name == team).OrderByDescending(game => game.PlayTime).ToList().ToViewModel();

        return View(model);
    }

    [Authorize(Policy = "TeamManagerOnly")]
    [HttpGet]
    public IActionResult NewGame()
    {
        var model = new NewGameViewModel();

        PrefillSelectOptions();

        return View(model);
    }

    private void PrefillSelectOptions()
    {
        var teams = _teamRepository.GetTeams();
        ViewBag.Teams = new SelectList(teams, "Id", "Name");

        var opponents = _opponentRepository.GetOpponents().Prepend(new Opponent() { Id = -1, Name = "Select opponent" });
        ViewBag.Opponents = new SelectList(opponents, "Id", "Name");

        var coaches = _coachRepository.GetCoaches().Prepend(new Coach() { Id = -1, Name = "Select a coach" });
        ViewBag.Coaches = new SelectList(coaches, "Id", "Name");

        var careTakers = _playerRepository.GetPlayers().SelectMany(p => p.CareTakers)
            .Prepend(new CareTaker { Id = -1, Name = "Select a caretaker" });
        ViewBag.CareTakers = new SelectList(careTakers, "Id", "Name");
    }

    [Authorize(Policy = "TeamManagerOnly")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> NewGame(NewGameViewModel newGameVM)
    {
        IActionResult result = null;

        if (ModelState.IsValid)
        {
            try
            {
                Game newGame = _gameService.CreateNewGame(
                    newGameVM.TeamId,
                    newGameVM.PlayTime,
                    newGameVM.DepartureTime,
                    newGameVM.IsHomeGame,
                    newGameVM.CoachId,
                    newGameVM.LaundryDutyId,
                    newGameVM.OpponentId);

                if (null != newGame)
                {
                    // Some error occurs when insering into the database. For SO&A 2
                    // not an issue, since it's more about design and code structure.
                    // Feel free to solve the bug :-)
                    //await _gameRepository.AddGame(newGame);

                    result = RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError(
                    "Error on creating Game",
                    e.Message);
            }
        }

        if (null == result)
        {
            PrefillSelectOptions();
            result = View(newGameVM);
        }

        return result; 

    }
}
