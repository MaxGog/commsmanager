using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Interfaces;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.ViewModels;

public partial class ArtistDetailViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private LocalArtistProfile _artist = new();

    [ObservableProperty]
    private bool _isNewArtist = true;

    [ObservableProperty]
    private bool _isSaving;

    public ArtistDetailViewModel(DatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;

        SaveCommand = new AsyncRelayCommand(SaveArtistAsync);
        CancelCommand = new AsyncRelayCommand(CancelAsync);
    }

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

    public void Initialize(LocalArtistProfile? artist = null)
    {
        if (artist == null)
        {
            Artist = new LocalArtistProfile
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow
            };
            IsNewArtist = true;
        }
        else
        {
            Artist = artist;
            IsNewArtist = false;
        }
    }

    private async Task SaveArtistAsync()
    {
        if (string.IsNullOrWhiteSpace(Artist.Name))
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Имя художника обязательно", "OK");
            return;
        }

        IsSaving = true;

        try
        {
            await _databaseService.SaveArtistProfileAsync(Artist);
            await _dialogService.ShowToastAsync("Художник сохранён");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Ошибка", $"Не удалось сохранить художника: {ex.Message}", "OK");
        }
        finally
        {
            IsSaving = false;
        }
    }

    private async Task CancelAsync()
    {
        bool confirmed = await _dialogService.ShowConfirmationAsync(
            "Отмена",
            "Вы уверены, что хотите отменить изменения?",
            "Да",
            "Нет");

        if (confirmed)
        {
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
