using Core.Domain;
using Core.DomainServices.Repos.Intf;
using Core.DomainServices.Services.Intf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Impl
{
    public class GameServiceBasic : IGameService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IOpponentRepository _opponentRepository;

        public GameServiceBasic(
            ITeamRepository teamRepository,
            ICoachRepository coachRepository,
            IPlayerRepository playerRepository,
            IOpponentRepository opponentRepository)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _coachRepository = coachRepository ?? throw new ArgumentNullException(nameof(coachRepository));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _opponentRepository = opponentRepository ?? throw new ArgumentNullException(nameof(opponentRepository));
        }

        public Game CreateNewGame(
             int teamId,
             DateTime playTime,
             DateTime? departureTime,
             bool isHomeGame,
             int coachId,
             int laundryDutyId,
             int opponentId)
        {
            Game newGame = new Game(playTime, isHomeGame);

            if(departureTime.HasValue)
            {
                newGame.DepartureTime = departureTime;
            }

            AssignTeam(newGame, teamId);

            AssignOpponent(newGame, opponentId);

            AssignCoach(newGame, teamId, coachId);

            // Before we implement the logic with respect to caretakers, first find out who all possible
            // caretakers are for this game.
            newGame.Players = _playerRepository.GetPlayers().ToList().OrderBy(p => p.Games.Count).Take(12).ToList();
            List<CareTaker> gamePossibleCareTakers = new List<CareTaker>();
            newGame.Players.ToList().ForEach(p => gamePossibleCareTakers.AddRange(p.CareTakers.ToList()));

            // Note: first assign drivers, then the laundry duty because selected as a driver saves you
            // from the laundry duty, even you were selected manyually (laundryDutyId).
            AssignDrivers(newGame, gamePossibleCareTakers);

            AssignLaundryDuty(newGame, laundryDutyId, gamePossibleCareTakers);

            return newGame;
        }

        private void AssignTeam(Game newGame, int teamId)
        {
            Team team= _teamRepository.GetTeams().Single(t => t.Id == teamId);
            newGame.Team = team;
        }

        private void AssignCoach(Game newGame, int teamId, int coachId)
        {
            // Business rule: assign the coach as was selected, if none, then select the
            // head coach.
            Coach selectedCoach = null;
            if (coachId == -1)
            {
                Team team = _teamRepository.GetTeams().SingleOrDefault(t => t.Id == teamId);
                selectedCoach = team.TeamHeadCoach;
            }
            else
            {
                selectedCoach = _coachRepository.GetById(coachId);
            }

            newGame.Coach = selectedCoach;
            newGame.CoachId = selectedCoach.Id;
        }

        private void AssignOpponent(Game newGame, int opponentId)
        {
            Opponent opponent = _opponentRepository.GetById(opponentId);
            newGame.Opponent = opponent;
        }

        private void AssignDrivers(Game newGame, IEnumerable<CareTaker> gamePossibleCareTakers)
        {
            // Business rule: select 4 drivers, a drivers license is required.
            IEnumerable<CareTaker> drivers = gamePossibleCareTakers.Where(ct => ct.HasCar).Take(4);
            newGame.Drivers = drivers.ToList();
        }

        private void AssignLaundryDuty(Game newGame, int laundryDutyId, IEnumerable<CareTaker> gamePossibleCareTakers)
        {
            CareTaker laundryDutyParent = null;

            // Business rule: if a parent was selected manually AND this parent is not as a driver for this game,
            // then select this parent.
            if (laundryDutyId != -1 && !newGame.Drivers.Any(ct => ct.Id == laundryDutyId))
            {
                laundryDutyParent = gamePossibleCareTakers.Single(ct => ct.Id == laundryDutyId);
            }
            else // select a random parent that was not selected as a driver.
            {
                IEnumerable<CareTaker> possibleLaundryCareTakers =
                    gamePossibleCareTakers.Where(ct => !newGame.Drivers.Contains(ct));

                laundryDutyParent = possibleLaundryCareTakers.FirstOrDefault();
            }

            newGame.LaundryDuty = laundryDutyParent;
            newGame.Id = laundryDutyParent.Id;
        }
    }
}
