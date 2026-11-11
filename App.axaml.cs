using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MusicSchool.Views;

namespace MusicSchool;

public partial class App : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Создаем главное окно
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}