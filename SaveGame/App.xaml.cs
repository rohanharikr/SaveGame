using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.ViewModels;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SaveGame.Controls;
using SaveGame.Views;

namespace SaveGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IGDBService>();
                    services.AddSingleton<GameStore>();
                    services.AddTransient<ModalNavigationStore>();

                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<HomeView>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<PlayView>();
                    services.AddSingleton<PlayViewModel>();
                    services.AddSingleton<PlayingView>();
                    services.AddSingleton<PlayingViewModel>();
                    services.AddSingleton<PlayedView>();
                    services.AddSingleton<PlayedViewModel>();
                    
                    services.AddSingleton(services => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
