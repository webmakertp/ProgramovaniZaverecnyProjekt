using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameTrackerApp.Views;

public partial class SessionFormView : UserControl
{
    public SessionFormView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
