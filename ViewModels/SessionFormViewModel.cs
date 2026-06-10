using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTrackerApp.Models;
using GameTrackerApp.Repositories;

namespace GameTrackerApp.ViewModels;

// formular pro pridani nebo upravu herni relace
public partial class SessionFormViewModel : ViewModelBase
{
    private readonly IGameSessionRepository _sessionRepo;
    private readonly GameSession? _existing;
    private readonly int _gameId;

    [ObservableProperty]
    private DateTimeOffset _datePlayed;

    [ObservableProperty]
    private decimal _hoursPlayed;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public event Action? NavigateBack;

    public IRelayCommand SaveCommand { get; }
    public IRelayCommand BackCommand { get; }

    public SessionFormViewModel(int gameId, GameSession? session = null)
    {
        _sessionRepo = Services.Get<IGameSessionRepository>();
        _existing = session;
        _gameId = gameId;

        SaveCommand = new RelayCommand(Save);
        BackCommand = new RelayCommand(() => NavigateBack?.Invoke());

        if (_existing != null)
        {
            DatePlayed = _existing.DatePlayed;
            HoursPlayed = _existing.HoursPlayed;
        }
        else
        {
            DatePlayed = DateTime.Today;
            HoursPlayed = 0.5m;
        }
    }

    private void Save()
    {
        if (DatePlayed.Date > DateTime.Today)
        {
            ErrorMessage = "Datum nesmí být v budoucnosti";
            return;
        }

        if (HoursPlayed <= 0)
        {
            ErrorMessage = "Hodiny musí být větší než 0";
            return;
        }

        if (HoursPlayed > 24)
        {
            ErrorMessage = "Hodiny nesmí přesáhnout 24";
            return;
        }

        ErrorMessage = string.Empty;

        var session = new GameSession
        {
            GameId = _gameId,
            DatePlayed = DatePlayed.Date,
            HoursPlayed = HoursPlayed
        };

        try
        {
            if (_existing == null)
            {
                _sessionRepo.Insert(session);
                StatusMessage = "relace uložena";
            }
            else
            {
                session.Id = _existing.Id;
                _sessionRepo.Update(session);
                StatusMessage = "relace upravena";
            }

            NavigateBack?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = $"CHYBA při uložení: {ex.Message}";
        }
    }
}
