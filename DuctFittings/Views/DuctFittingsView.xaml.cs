using Wpf.Ui.Appearance;

namespace DuctFittings.Views;

public sealed partial class DuctFittingsView
{
    public DuctFittingsView()
    {
        ApplicationThemeManager.Apply(this);
        InitializeComponent();
        SystemThemeWatcher.Watch(this);
    }
}