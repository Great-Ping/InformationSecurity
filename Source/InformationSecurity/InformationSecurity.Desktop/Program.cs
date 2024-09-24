using System;
using Microsoft.Extensions.DependencyInjection;
using Avalonia;
using Avalonia.ReactiveUI;
using InformationSecurity;
using InformationSecurity.Application;


IServiceCollection serviceCollection = new ServiceCollection();
    serviceCollection.UseCommonServices();

IServiceProvider services = serviceCollection.BuildServiceProvider();

AppBuilder app = AppBuilder
    .Configure(() => new App(
            services.GetRequiredService<MainViewModel>()
        )
    )
    .UsePlatformDetect()
    .WithInterFont()
    .LogToTrace()
    .UseReactiveUI();

app.StartWithClassicDesktopLifetime(args);
