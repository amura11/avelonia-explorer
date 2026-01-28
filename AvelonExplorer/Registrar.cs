using AvelonExplorer.Services;
using AvelonExplorer.ViewModels;
using AvelonExplorer.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace AvelonExplorer;

public static class Registrar
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        RegisterDomainServices(services);
        RegisterFactories(services);
        RegisterModels(services);
        
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
        
        return services;
    }
    
    private static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddTransient<IFileSystemService, FileSystemService>();
        services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
    }

    private static void RegisterFactories(IServiceCollection services)
    {
        services.AddTransient<IFileSystemTabViewModelFactory, FileSystemTabViewModelFactory>();
    }

    private static void RegisterModels(IServiceCollection services)
    {
        
    }
}