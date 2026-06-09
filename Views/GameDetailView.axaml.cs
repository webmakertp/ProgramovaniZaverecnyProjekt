using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameTrackerApp.Views;

public partial class GameDetailView : UserControl
{
    public GameDetailView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
