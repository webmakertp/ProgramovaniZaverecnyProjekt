using System;

namespace GameTrackerApp.Models;

// jedna herni relace, kdy jsem hral kolik hodin
public class GameSession
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public DateTime DatePlayed { get; set; }
    public decimal HoursPlayed { get; set; }
}
