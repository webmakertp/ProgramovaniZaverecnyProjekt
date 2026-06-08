using System;
using GameTrackerApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameTrackerApp.ViewModels;

// TODO: dokoncit v kroku 7
public partial class GameFormViewModel : ViewModelBase
{
    public event Action? NavigateBack;

    public GameFormViewModel(Game? game = null)
    {
        // zatim prazdny, dodela se v dalsim kroku
    }
}
