using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Intf
{
    public interface IGameService
    {
        Game CreateNewGame(
            int TeamId,
            DateTime PlayTime,
            DateTime? DepartureTime,
            bool IsHomeGame,
            int CoachId,
            int LaundryDutyId,
            int opponentId);
    }
}
