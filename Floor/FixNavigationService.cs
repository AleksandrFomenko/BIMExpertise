using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace Floor;



public partial class FixNavigationService(INavigationViewPageProvider pageProvider) : INavigationService
{
    protected INavigationView? NavigationControl { get; set; }

    /// <inheritdoc />
    public INavigationView GetNavigationControl()
    {
        return NavigationControl ?? throw new ArgumentNullException(nameof(NavigationControl));
    }

    /// <inheritdoc />
    public void SetNavigationControl(INavigationView navigation)
    {
        NavigationControl = navigation;
        NavigationControl.SetPageProviderService(pageProvider);
    }

    /// <inheritdoc />
    public bool Navigate(Type pageType)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageType);
    }

    /// <inheritdoc />
    public bool Navigate(Type pageType, object? dataContext)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageType, dataContext);
    }

    /// <inheritdoc />
    public bool Navigate(string pageTag)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageTag);
    }

    /// <inheritdoc />
    public bool Navigate(string pageTag, object? dataContext)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.Navigate(pageTag, dataContext);
    }

    /// <inheritdoc />
    public bool GoBack()
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.GoBack();
    }

    /// <inheritdoc />
    public bool NavigateWithHierarchy(Type pageType)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.NavigateWithHierarchy(pageType);
    }

    /// <inheritdoc />
    public bool NavigateWithHierarchy(Type pageType, object? dataContext)
    {
        ThrowIfNavigationControlIsNull();

        return NavigationControl!.NavigateWithHierarchy(pageType, dataContext);
    }

    protected void ThrowIfNavigationControlIsNull()
    {
        if (NavigationControl is null)
        {
            throw new ArgumentNullException(nameof(NavigationControl));
        }
    }
}