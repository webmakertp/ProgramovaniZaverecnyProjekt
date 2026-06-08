using System;
using System.IO;
using GameTrackerApp.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GameTrackerApp;

// trida pro nastaveni DI, tady se registruji vsechny repo
public static class Services
{
    public static IServiceProvider Provider { get; private set; } = null!;

    public static void Init()
    {
        // nacteni .env souboru, kdyby nebyl tak to spadne ale to je ok
        var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
        if (File.Exists(envPath))
        {
            DotNetEnv.Env.Load(envPath);
        }
        else
        {
            // fallback kdyz to bezi z ide, hledame v rootu projektu
            var fallback = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env");
            if (File.Exists(fallback))
                DotNetEnv.Env.Load(fallback);
        }

        var connString = DotNetEnv.Env.GetString("DB_CONNECTION_STRING");
        // bacha na null u tohodle, kdyz neco chybi v .env tak to bude prazdny
        if (string.IsNullOrEmpty(connString))
            throw new InvalidOperationException("chybi DB_CONNECTION_STRING v .env");

        var services = new ServiceCollection();

        // registrace repo jako singleton, jedna instance pro celou app
        services.AddSingleton<IGameRepository>(_ => new GameRepository(connString));
        services.AddSingleton<IGameSessionRepository>(_ => new GameSessionRepository(connString));
        services.AddSingleton<IPlatformRepository>(_ => new PlatformRepository(connString));

        Provider = services.BuildServiceProvider();
    }

    // pomocna metoda aby se to lip psalo ve viewmodelech
    public static T Get<T>() where T : notnull
    {
        return Provider.GetRequiredService<T>();
    }
}
