using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class OrdersPage : ContentPage
{
    public OrdersPage() : this(App.Services.GetRequiredService<OrdersViewModel>())
    {
    }

    public OrdersPage(OrdersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is OrdersViewModel viewModel)
        {
            await viewModel.LoadOrdersCommand.ExecuteAsync(null);
        }
    }
}
