using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameTrackerApp.Views;

public partial class ConfirmDialogView : UserControl
{
    public ConfirmDialogView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
