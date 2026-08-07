using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.ViewModels;

public partial class ArtistsViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private List<LocalArtistProfile> _artists = new();

    [ObservableProperty]
    private List<LocalCommission> _commissions = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private LocalArtistProfile? _selectedArtist;

    public ArtistsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        LoadArtistsCommand = new AsyncRelayCommand(LoadArtistsAsync);
        AddArtistCommand = new AsyncRelayCommand(AddArtistAsync);
        DeleteArtistCommand = new AsyncRelayCommand<LocalArtistProfile>(DeleteArtistAsync);
        EditArtistCommand = new AsyncRelayCommand<LocalArtistProfile>(EditArtistAsync);
        ViewCommissionsCommand = new AsyncRelayCommand<LocalArtistProfile>(ViewCommissionsAsync);
    }

    public IAsyncRelayCommand LoadArtistsCommand { get; }
    public IAsyncRelayCommand AddArtistCommand { get; }
    public IAsyncRelayCommand<LocalArtistProfile> DeleteArtistCommand { get; }
    public IAsyncRelayCommand<LocalArtistProfile> EditArtistCommand { get; }
    public IAsyncRelayCommand<LocalArtistProfile> ViewCommissionsCommand { get; }

    private async Task LoadArtistsAsync()
    {
        IsLoading = true;

        try
        {
            var allArtists = await _databaseService.GetArtistProfilesAsync();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Artists = allArtists;
            }
            else
            {
                Artists = [.. allArtists
                    .Where(a => a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                               a.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true)];
            }

            if (SelectedArtist != null)
            {
                Commissions = await _databaseService.GetCommissionsByArtistAsync(SelectedArtist.Id);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddArtistAsync()
    {
        await Shell.Current.GoToAsync("ArtistDetailPage");
    }

    private async Task EditArtistAsync(LocalArtistProfile? artist)
    {
        if (artist == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "Artist", artist }
        };

        await Shell.Current.GoToAsync("ArtistDetailPage", parameters);
    }

    [Obsolete]
    private async Task DeleteArtistAsync(LocalArtistProfile? artist)
    {
        if (artist == null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Удаление художника",
            $"Вы уверены, что хотите удалить художника {artist.Name}?",
            "Удалить",
            "Отмена");

        if (confirmed)
        {
            //await _databaseService.DeleteArtistAsync(artist);
            await LoadArtistsAsync();
        }
    }

    private async Task ViewCommissionsAsync(LocalArtistProfile? artist)
    {
        if (artist == null) return;

        SelectedArtist = artist;
        Commissions = await _databaseService.GetCommissionsByArtistAsync(artist.Id);
    }

    [RelayCommand]
    private void Search()
    {
        LoadArtistsCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    [Obsolete]
    private async Task AddCommissionAsync()
    {
        if (SelectedArtist == null)
        {
            await Shell.Current.DisplayAlert("Внимание", "Сначала выберите художника", "OK");
            return;
        }

        await Shell.Current.DisplayAlert("В разработке", "Добавление комиссии пока не реализовано.", "OK");
    }
}