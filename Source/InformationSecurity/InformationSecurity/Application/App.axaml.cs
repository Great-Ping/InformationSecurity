using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using InformationSecurity.Pages.Cryptography;

namespace InformationSecurity.Application;

public partial class App : Avalonia.Application
{
    private readonly MainViewModel _mainViewModel;
    
    public App(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new CryptographyPage()
            {
                DataContext = _mainViewModel.CryptographyPage
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}