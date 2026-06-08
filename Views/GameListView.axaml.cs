using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameTrackerApp.Views;

// code behind pro seznam her, je tu jen inicializace
public partial class GameListView : UserControl
{
    public GameListView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
