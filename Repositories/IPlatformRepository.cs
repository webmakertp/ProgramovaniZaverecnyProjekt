using System.Collections.Generic;
using GameTrackerApp.Models;

namespace GameTrackerApp.Repositories;

// interface pro ciselnik platform
public interface IPlatformRepository
{
    // vrat vsechny platformy, pouziva se v comboboxu
    List<Platform> GetAll();
}
