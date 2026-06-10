using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTrackerApp.Models;
using GameTrackerApp.Repositories;

namespace GameTrackerApp.ViewModels;

// viewmodel pro seznam her, hlavni obrazovka po startu
public partial class GameListViewModel : ViewModelBase
{
    private readonly IGameRepository _gameRepo;
    private List<Game> _loadedGames = new();

    [ObservableProperty]
    private Game? _selectedGame;

    [ObservableProperty]
    private string _statusMessage = "načítám...";

    [ObservableProperty]
    private bool _isDialogOpen;

    [ObservableProperty]
    private ViewModelBase? _dialogViewModel;

    [ObservableProperty]
    private string _selectedSort = "Název";

    [ObservableProperty]
    private bool _sortAscending = true;

    public ObservableCollection<Game> Games { get; } = new();

    public List<string> SortOptions { get; } = new()
    {
        "Název",
        "Rok vydání",
        "Vývojář",
        "Platforma"
    };

    public string SortDirectionText => SortAscending ? "↑ vzestupně" : "↓ sestupně";

    // prikazy pro tlacitka
    public IRelayCommand RefreshCommand { get; }
    public IRelayCommand AddCommand { get; }
    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand DetailCommand { get; }
    public IRelayCommand ToggleSortCommand { get; }

    // event pro navigaci, mainwindow to zachyti
    public event Action<ViewModelBase>? Navigate;
    public event Action<int>? NavigateToDetail;

    public GameListViewModel()
    {
        _gameRepo = Services.Get<IGameRepository>();

        RefreshCommand = new RelayCommand(LoadGames);
        AddCommand = new RelayCommand(() => Navigate?.Invoke(new GameFormViewModel()));
        DeleteCommand = new RelayCommand(DeleteSelected);
        DetailCommand = new RelayCommand(() =>
        {
            if (SelectedGame != null)
                NavigateToDetail?.Invoke(SelectedGame.Id);
        });
        ToggleSortCommand = new RelayCommand(() => SortAscending = !SortAscending);

        LoadGames();
    }

    partial void OnSelectedSortChanged(string value) => ApplySort();
    partial void OnSortAscendingChanged(bool value) => ApplySort();

    private void LoadGames()
    {
        try
        {
            _loadedGames = _gameRepo.GetAll();
            StatusMessage = $"načteno {_loadedGames.Count} her";
            ApplySort();
        }
        catch (Exception ex)
        {
            StatusMessage = $"CHYBA: {ex.Message}";
        }
    }

    private void ApplySort()
    {
        Games.Clear();

        IEnumerable<Game> sorted = SelectedSort switch
        {
            "Rok vydání" => _loadedGames.OrderBy(g => g.ReleaseYear),
            "Vývojář" => _loadedGames.OrderBy(g => g.Developer),
            "Platforma" => _loadedGames.OrderBy(g => g.PlatformName ?? string.Empty),
            _ => _loadedGames.OrderBy(g => g.Title)
        };

        if (!SortAscending)
            sorted = sorted.Reverse();

        foreach (var g in sorted)
            Games.Add(g);

        OnPropertyChanged(nameof(SortDirectionText));
    }

    private void DeleteSelected()
    {
        if (SelectedGame == null) return;

        var confirm = new ConfirmDialogViewModel($"Opravdu chcete smazat hru '{SelectedGame.Title}'?");
        confirm.Result += approved =>
        {
            IsDialogOpen = false;
            DialogViewModel = null;
            if (approved)
            {
                _gameRepo.Delete(SelectedGame.Id);
                LoadGames();
            }
        };
        DialogViewModel = confirm;
        IsDialogOpen = true;
    }
}
