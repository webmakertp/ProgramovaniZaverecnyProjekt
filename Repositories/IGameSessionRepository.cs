using System.Collections.Generic;
using GameTrackerApp.Models;

namespace GameTrackerApp.Repositories;

// interface pro sessiony, operace nad hernima relacema
public interface IGameSessionRepository
{
    List<GameSession> GetByGameId(int gameId);
    GameSession? GetById(int id);
    void Insert(GameSession session);
    void Update(GameSession session);
    void Delete(int id);
}
