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

    [ObservableProperty]
    private bool _isDialogOpen;

    [ObservableProperty]
    private ViewModelBase? _dialogViewModel;

    public ObservableCollection<GameSession> Sessions { get; } = new();

    public event Action? NavigateBack;
    public event Action<Game?>? NavigateToEdit;
    public event Action<int, GameSession?>? NavigateToSessionForm;

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
        NavigateToSessionForm?.Invoke(Game.Id, null);
    }

    private void EditSession()
    {
        if (SelectedSession == null) return;
        NavigateToSessionForm?.Invoke(Game.Id, SelectedSession);
    }

    private void DeleteSession()
    {
        if (SelectedSession == null) return;

        var confirm = new ConfirmDialogViewModel("Opravdu chcete smazat tuto relaci?");
        confirm.Result += approved =>
        {
            IsDialogOpen = false;
            DialogViewModel = null;
            if (approved)
            {
                _sessionRepo.Delete(SelectedSession.Id);
                Load(Game.Id);
            }
        };
        DialogViewModel = confirm;
        IsDialogOpen = true;
    }
}
