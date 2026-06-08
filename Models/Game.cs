using System.Collections.Generic;

namespace GameTrackerApp.Models;

// hlavni entita hra, obsahuje zakladni info
public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Developer { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int? PlatformId { get; set; }
    public string? PlatformName { get; set; } // tady to taham z joinu, nemusi byt vzdy

    // sessiony se nacitaj zvlast, neni to efektivni ale je to prehledny
    public List<GameSession> Sessions { get; set; } = new();
}
