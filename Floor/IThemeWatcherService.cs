using System.Windows;

namespace Floor;

public interface IThemeWatcherService
{
    void Initialize();
    void ApplyTheme();
    void Watch(FrameworkElement frameworkElement);
    void Unwatch();
}