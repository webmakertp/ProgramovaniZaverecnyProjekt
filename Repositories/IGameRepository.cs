using System.Collections.Generic;
using GameTrackerApp.Models;

namespace GameTrackerApp.Repositories;

// interface pro zakladni crud nad hrama
public interface IGameRepository
{
    List<Game> GetAll();
    Game? GetById(int id);
    int Insert(Game game);
    void Update(Game game);
    void Delete(int id);
}
