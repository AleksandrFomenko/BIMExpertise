using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace WpfResourcesBimExpertise.Services.Appearance;

    public static class ThemeWatcherService
    {
        private static readonly List<FrameworkElement> _observedElements = new List<FrameworkElement>();
        
        static ThemeWatcherService()
        {
            TryOverridePageMetadata();
        }

        public static void Initialize()
        {
            UiApplication.Current.Resources = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/WpfResourcesBimExpertise;component/Styles/App.Resources.xaml", UriKind.Absolute)
            };
            ApplicationThemeManager.Changed += OnApplicationThemeManagerChanged;
        }

        public static void ApplyTheme(ApplicationTheme theme)
        {
            ApplicationThemeManager.Apply(theme);
            UpdateBackground(theme);
        }

        private static void OnApplicationThemeManagerChanged(ApplicationTheme currentApplicationTheme, System.Windows.Media.Color systemAccent)
        {
            foreach (var frameworkElement in _observedElements)
            {
                ApplicationThemeManager.Apply(frameworkElement);
                UpdateDictionary(frameworkElement);
            }
        }

        private static void UpdateDictionary(FrameworkElement frameworkElement)
        {
            var themedResources = frameworkElement.Resources.MergedDictionaries
                .Where(dictionary => dictionary.Source?.OriginalString.Contains("WpfResourcesBimExpertise;", StringComparison.OrdinalIgnoreCase) == true)
                .ToArray();

            if (UiApplication.Current.Resources.MergedDictionaries.Count >= 2)
            {
                frameworkElement.Resources.MergedDictionaries.Insert(0, UiApplication.Current.Resources.MergedDictionaries[0]);
                frameworkElement.Resources.MergedDictionaries.Insert(1, UiApplication.Current.Resources.MergedDictionaries[1]);
            }

            foreach (var themedResource in themedResources)
            {
                frameworkElement.Resources.MergedDictionaries.Remove(themedResource);
            }
        }

        public static void Watch(FrameworkElement frameworkElement)
        {
            ApplicationThemeManager.Apply(frameworkElement);
            frameworkElement.Loaded += OnWatchedElementLoaded;
            frameworkElement.Unloaded += OnWatchedElementUnloaded;
        }

        private static void OnWatchedElementLoaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            _observedElements.Add(element);

            if (element.Resources.MergedDictionaries.Count > 0 && 
                element.Resources.MergedDictionaries[0].Source?.OriginalString != UiApplication.Current.Resources.MergedDictionaries[0].Source.OriginalString)
            {
                ApplicationThemeManager.Apply(element);
                UpdateDictionary(element);
            }
        }

        private static void OnWatchedElementUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            _observedElements.Remove(element);
        }

        private static void UpdateBackground(ApplicationTheme theme)
        {
            foreach (var window in _observedElements.Select(Window.GetWindow).Distinct())
            {
                WindowBackgroundManager.UpdateBackground(window, theme, WindowBackdropType.Mica);
            }
        }
        
        private static void TryOverridePageMetadata()
        {
            var dp = FrameworkElement.OverridesDefaultStyleProperty;
            var metadata = dp.GetMetadata(typeof(Page)) as FrameworkPropertyMetadata;
            
            if (metadata != null && metadata.DefaultValue is Type currentDefault && currentDefault == typeof(Page))
            {
                try
                {
                    dp.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(typeof(NavigationViewContentPresenter)));
                }
                catch (Exception ex)
                {
                    //Ignored
                }
            }

        }
    }