using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class OrderDetailPage : ContentPage, IQueryAttributable
{
    private readonly OrderDetailViewModel _viewModel;

    public OrderDetailPage() : this(App.Services.GetRequiredService<OrderDetailViewModel>())
    {
    }

    public OrderDetailPage(OrderDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCustomersAndArtistsAsync();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Order", out var orderObject) && orderObject is LocalOrder order)
        {
            _viewModel.Initialize(order);
        }
        else
        {
            _viewModel.Initialize();
        }
    }
}
