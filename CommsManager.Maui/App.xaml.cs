using CommsManager.Maui.Interfaces;
using CommsManager.Maui.Services;
using CommsManager.Maui.Views;

namespace CommsManager;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    public static IServiceProvider Services { get; private set; } = null!;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        Services = serviceProvider;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell()) { Title = "CommsManager" };
    }

    protected override async void OnStart()
    {
        base.OnStart();

        await InitializeDatabaseAsync();

        await CleanupTempFilesAsync();
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        Task.Run(async () =>
        {
            var backupService = _serviceProvider.GetService<IDatabaseBackupService>();
            if (backupService != null)
            {
                await backupService.CreateBackupAsync();
            }
        });
    }

    [Obsolete]
    private async Task InitializeDatabaseAsync()
    {
        try
        {
            var initializer = _serviceProvider.GetService<IDatabaseInitializer>();
            if (initializer != null)
            {
                await initializer.InitializeAsync();
            }

            var migrator = _serviceProvider.GetService<IDatabaseMigrator>();
            if (migrator != null)
            {
                var currentVersion = await migrator.GetCurrentVersionAsync();
                if (currentVersion < 2)
                {
                    await migrator.MigrateAsync(currentVersion, 2);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            await Shell.Current.DisplayAlert("Ошибка",
                "Не удалось инициализировать базу данных", "OK");
        }
    }

    private async Task CleanupTempFilesAsync()
    {
        var fileService = _serviceProvider.GetService<IFileService>();
        if (fileService != null)
        {
            await fileService.CleanupTempFilesAsync();
        }
    }
}
