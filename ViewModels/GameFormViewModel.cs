using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTrackerApp.Models;
using GameTrackerApp.Repositories;

namespace GameTrackerApp.ViewModels;

// formular pro pridani nebo upravu hry, obsahuje combobox pro platformu
public partial class GameFormViewModel : ViewModelBase
{
    private readonly IGameRepository _gameRepo;
    private readonly IPlatformRepository _platformRepo;
    private readonly Game? _existing;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _developer = string.Empty;

    [ObservableProperty]
    private int _releaseYear;

    [ObservableProperty]
    private Platform? _selectedPlatform;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ObservableCollection<Platform> Platforms { get; } = new();

    public event Action? NavigateBack;

    public IRelayCommand SaveCommand { get; }
    public IRelayCommand BackCommand { get; }

    public GameFormViewModel(Game? game = null)
    {
        _gameRepo = Services.Get<IGameRepository>();
        _platformRepo = Services.Get<IPlatformRepository>();
        _existing = game;

        SaveCommand = new RelayCommand(Save);
        BackCommand = new RelayCommand(() => NavigateBack?.Invoke());

        LoadPlatforms();

        if (_existing != null)
        {
            Title = _existing.Title;
            Developer = _existing.Developer;
            ReleaseYear = _existing.ReleaseYear;
            // najdeme platformu v seznamu podle id
            foreach (var p in Platforms)
            {
                if (_existing.PlatformId.HasValue && p.Id == _existing.PlatformId.Value)
                {
                    SelectedPlatform = p;
                    break;
                }
            }
        }
    }

    private void LoadPlatforms()
    {
        Platforms.Clear();
        var list = _platformRepo.GetAll();
        foreach (var p in list)
            Platforms.Add(p);
    }

    private void Save()
    {
        // zakladni validace, nic slozityho
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "nazev hry nesmi byt prazdny";
            return;
        }

        if (ReleaseYear < 1970 || ReleaseYear > 2100)
        {
            ErrorMessage = "rok musi byt mezi 1970 a 2100";
            return;
        }

        ErrorMessage = string.Empty;

        var game = new Game
        {
            Title = Title.Trim(),
            Developer = Developer.Trim(),
            ReleaseYear = ReleaseYear,
            PlatformId = SelectedPlatform?.Id
        };

        if (_existing == null)
        {
            _gameRepo.Insert(game);
        }
        else
        {
            game.Id = _existing.Id;
            _gameRepo.Update(game);
        }

        NavigateBack?.Invoke();
    }
}
