using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage() : this(App.Services.GetRequiredService<ProfileViewModel>())
    {
    }

    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProfileViewModel viewModel)
        {
            await viewModel.LoadStatsCommand.ExecuteAsync(null);
        }
    }
}
