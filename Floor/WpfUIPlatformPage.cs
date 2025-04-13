using System.Windows.Controls;
using System.Windows;

public class WpfUIPlatformPage : Page
{
    static WpfUIPlatformPage()
    {
        ScrollViewer.HorizontalScrollBarVisibilityProperty.OverrideMetadata(
            typeof(WpfUIPlatformPage), 
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled));

        ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(
            typeof(WpfUIPlatformPage), 
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled));
        
        ScrollViewer.CanContentScrollProperty.OverrideMetadata(
            typeof(WpfUIPlatformPage), 
            new FrameworkPropertyMetadata(false));
    }
    
}