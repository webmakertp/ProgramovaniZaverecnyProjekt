using System;
using GameTrackerApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameTrackerApp.ViewModels;

// TODO: dokoncit v kroku 6
public partial class GameDetailViewModel : ViewModelBase
{
    public event Action? NavigateBack;
    public event Action<Game?>? NavigateToEdit;

    public GameDetailViewModel(int gameId)
    {
        // zatim prazdny, dodela se v dalsim kroku
    }
}
