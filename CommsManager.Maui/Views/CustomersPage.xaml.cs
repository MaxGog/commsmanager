using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class CustomersPage : ContentPage
{
    public CustomersPage() : this(App.Services.GetRequiredService<CustomersViewModel>())
    {
    }

    public CustomersPage(CustomersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CustomersViewModel viewModel)
        {
            await viewModel.LoadCustomersCommand.ExecuteAsync(null);
        }
    }
}
