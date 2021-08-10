using System;
using KaraokeLine.Interfaces;
using KaraokeLine.Services;
using KaraokeLine.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;

namespace KaraokeLine.Configuration
{
    public class ContainerManager
    {
        public static ContainerManager Instance { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            if (Instance is null)
                Instance = new ContainerManager();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ConfigureViews(serviceCollection);
            ConfigureViewModels(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICache, Cache>()
                             .AddSingleton<ISessionService, SessionService>()
                             .AddSingleton<IPlayerService, PlayerService>()
                             .AddSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();
        }

        private static void ConfigureViews(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MainWindow>();
        }

        private static void ConfigureViewModels(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MainWindowVm>();
        }

        public static T GetInstance<T>() => ServiceProvider.GetRequiredService<T>();
    }
}