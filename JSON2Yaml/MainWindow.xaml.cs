using CommunityToolkit.Mvvm.DependencyInjection;
using Wpf.Ui.Controls;

namespace JSON2Yaml
{
    public partial class MainWindow: FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);
            DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
        }
    }
}
