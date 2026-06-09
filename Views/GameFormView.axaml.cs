using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameTrackerApp.Views;

public partial class GameFormView : UserControl
{
    public GameFormView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
