using System.ComponentModel.DataAnnotations;

namespace Portal.Models;

public class NewGameViewModel
{
    public int TeamId { get; set; }

    public DateTime PlayTime { get; set; } = DateTime.Now.AddDays(1);

    public DateTime? DepartureTime { get; set; }

    public bool IsHomeGame { get; set; }

    public int CoachId { get; set; }

    public int LaundryDutyId { get; set; }

    [Required]
    public int OpponentId { get; set; }
}