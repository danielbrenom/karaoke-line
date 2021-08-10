using System.IO;
using System.Windows;
using KaraokeLine.Configuration;
using Microsoft.Extensions.Configuration;
using ShowMeTheXAML;

namespace KaraokeLine
{
    public partial class App : Application
    {
        private IConfiguration Configuration { get; set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            XamlDisplay.Init();
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
            ContainerManager.Init();
            
            var mainWindow = ContainerManager.GetInstance<MainWindow>();
            mainWindow.Show();
        }
    }
}