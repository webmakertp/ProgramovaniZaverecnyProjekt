using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTrackerApp.Models;
using GameTrackerApp.Repositories;

namespace GameTrackerApp.ViewModels;

// viewmodel pro seznam her, hlavni obrazovka po startu
public partial class GameListViewModel : ViewModelBase
{
    private readonly IGameRepository _gameRepo;

    [ObservableProperty]
    private Game? _selectedGame;

    [ObservableProperty]
    private string _statusMessage = "nacitam...";

    public ObservableCollection<Game> Games { get; } = new();

    // prikazy pro tlacitka
    public IRelayCommand RefreshCommand { get; }
    public IRelayCommand AddCommand { get; }
    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand DetailCommand { get; }

    // event pro navigaci, mainwindow to zachyti
    public event Action<ViewModelBase>? Navigate;
    public event Action<int>? NavigateToDetail;

    public GameListViewModel()
    {
        // tady to bere z DI, kdyby to bylo null tak je to spatne
        _gameRepo = Services.Get<IGameRepository>();

        RefreshCommand = new RelayCommand(LoadGames);
        AddCommand = new RelayCommand(() => Navigate?.Invoke(new GameFormViewModel()));
        DeleteCommand = new RelayCommand(DeleteSelected);
        DetailCommand = new RelayCommand(() =>
        {
            if (SelectedGame != null)
                NavigateToDetail?.Invoke(SelectedGame.Id);
        });

        LoadGames();
    }

    private void LoadGames()
    {
        try
        {
            Games.Clear();
            var list = _gameRepo.GetAll();
            StatusMessage = $"nacteno {list.Count} her";
            foreach (var g in list)
                Games.Add(g);
        }
        catch (Exception ex)
        {
            StatusMessage = $"CHYBA: {ex.Message}";
        }
    }

    private void DeleteSelected()
    {
        if (SelectedGame == null) return;

        // TODO: pridat nejaky confirmation dialog
        _gameRepo.Delete(SelectedGame.Id);
        LoadGames();
    }
}
