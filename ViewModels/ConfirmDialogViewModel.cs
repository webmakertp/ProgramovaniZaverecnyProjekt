using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GameTrackerApp.ViewModels;

// jednoduchy dialog na potvrzeni ano/ne
public partial class ConfirmDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _message = string.Empty;

    public event Action<bool>? Result;

    public IRelayCommand YesCommand { get; }
    public IRelayCommand NoCommand { get; }

    public ConfirmDialogViewModel(string message)
    {
        _message = message;
        YesCommand = new RelayCommand(() => Result?.Invoke(true));
        NoCommand = new RelayCommand(() => Result?.Invoke(false));
    }
}
