using Microsoft.EntityFrameworkCore.Infrastructure;
using SaveGame.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SaveGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseFacade facade = new(new SQLiteService());
            facade.EnsureCreated();
        }
    }

}
