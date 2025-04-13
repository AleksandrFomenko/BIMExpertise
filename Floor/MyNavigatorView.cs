using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls; // пространство имен WPF UI

namespace Floor.Controls
{
    public class MyNavigationView : ContentControl
    {
        // Флаг, гарантирующий однократную инициализацию (если понадобится)
        private static bool _isInitialized;

        static MyNavigationView()
        {
            // Один раз принудительно инициируем NavigationViewContentPresenter,
            // что вызовет статический конструктор и OverrideMetadata.
            if (!_isInitialized)
            {
                var dummy = typeof(NavigationViewContentPresenter);
                _isInitialized = true;
            }

            // Обеспечиваем, чтобы наш контрол искался как MyNavigationView в стилях
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyNavigationView),
                new FrameworkPropertyMetadata(typeof(MyNavigationView)));
        }

        public MyNavigationView()
        {
            // В конструкторе создаём внутренний NavigationView.
            // Здесь можно добавить дополнительные настройки или подписки.
            NavigationView = new NavigationView();
            this.Content = NavigationView;
        }

        /// <summary>
        /// Внутренний NavigationView, который фактически используется для навигации.
        /// </summary>
        public NavigationView NavigationView { get; }
    }
}