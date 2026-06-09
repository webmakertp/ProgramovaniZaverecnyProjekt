using CommunityToolkit.Mvvm.ComponentModel;
using GameTrackerApp.Models;

namespace GameTrackerApp.ViewModels;

// hlavni viewmodel co ridi navigaci mezi obrazovkama
public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;

    public MainWindowViewModel()
    {
        ShowGameList();
    }

    private void ShowGameList()
    {
        var vm = new GameListViewModel();
        vm.Navigate += vm2 =>
        {
            if (vm2 is GameFormViewModel formVm)
                formVm.NavigateBack += ShowGameList;
            CurrentView = vm2;
        };
        vm.NavigateToDetail += id => ShowGameDetail(id);
        CurrentView = vm;
    }

    private void ShowGameDetail(int id)
    {
        var vm = new GameDetailViewModel(id);
        vm.NavigateBack += ShowGameList;
        vm.NavigateToEdit += g => ShowGameForm(g);
        CurrentView = vm;
    }

    private void ShowGameForm(Game? game)
    {
        var vm = new GameFormViewModel(game);
        vm.NavigateBack += ShowGameList;
        CurrentView = vm;
    }
}
