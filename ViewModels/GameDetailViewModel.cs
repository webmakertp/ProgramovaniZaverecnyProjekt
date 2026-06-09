using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTrackerApp.Models;
using GameTrackerApp.Repositories;

namespace GameTrackerApp.ViewModels;

// detail hry a crud nad sessionama
public partial class GameDetailViewModel : ViewModelBase
{
    private readonly IGameRepository _gameRepo;
    private readonly IGameSessionRepository _sessionRepo;

    [ObservableProperty]
    private Game _game = null!;

    [ObservableProperty]
    private GameSession? _selectedSession;

    public ObservableCollection<GameSession> Sessions { get; } = new();

    public event Action? NavigateBack;
    public event Action<Game?>? NavigateToEdit;

    public IRelayCommand BackCommand { get; }
    public IRelayCommand EditGameCommand { get; }
    public IRelayCommand AddSessionCommand { get; }
    public IRelayCommand EditSessionCommand { get; }
    public IRelayCommand DeleteSessionCommand { get; }

    public GameDetailViewModel(int gameId)
    {
        _gameRepo = Services.Get<IGameRepository>();
        _sessionRepo = Services.Get<IGameSessionRepository>();

        BackCommand = new RelayCommand(() => NavigateBack?.Invoke());
        EditGameCommand = new RelayCommand(() => NavigateToEdit?.Invoke(Game));
        AddSessionCommand = new RelayCommand(AddSession);
        EditSessionCommand = new RelayCommand(EditSession);
        DeleteSessionCommand = new RelayCommand(DeleteSession);

        Load(gameId);
    }

    private void Load(int gameId)
    {
        var g = _gameRepo.GetById(gameId);
        if (g == null) return;

        Game = g;

        Sessions.Clear();
        var list = _sessionRepo.GetByGameId(gameId);
        foreach (var s in list)
            Sessions.Add(s);
    }

    private void AddSession()
    {
        // pridani nove session, datum je dneska a hodiny 0
        var session = new GameSession
        {
            GameId = Game.Id,
            DatePlayed = DateTime.Today,
            HoursPlayed = 0
        };
        _sessionRepo.Insert(session);
        Load(Game.Id);
    }

    private void EditSession()
    {
        if (SelectedSession == null) return;

        // tady by mel byt nejaky dialog ale ja to jen zvysim hodiny o 1 jako demo
        // TODO: udelat normalni editaci
        SelectedSession.HoursPlayed += 1;
        _sessionRepo.Update(SelectedSession);
        Load(Game.Id);
    }

    private void DeleteSession()
    {
        if (SelectedSession == null) return;
        _sessionRepo.Delete(SelectedSession.Id);
        Load(Game.Id);
    }
}
