using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class ArtistsPage : ContentPage
{
    public ArtistsPage() : this(App.Services.GetRequiredService<ArtistsViewModel>())
    {
    }

    public ArtistsPage(ArtistsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ArtistsViewModel viewModel)
        {
            await viewModel.LoadArtistsCommand.ExecuteAsync(null);
        }
    }
}
