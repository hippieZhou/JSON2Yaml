using CommunityToolkit.Mvvm.DependencyInjection;
using JSON2Yaml.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace JSON2Yaml
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<IYamlService, YamlService>()
                .AddSingleton<MainViewModel>()
                .BuildServiceProvider());
            base.OnStartup(e);
        }
    }
}
