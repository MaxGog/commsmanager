using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.ViewModels;
using CommsManager.Maui.Data.Models;

namespace CommsManager.Maui.Views;

public partial class CustomerDetailPage : ContentPage, IQueryAttributable
{
    private readonly CustomerDetailViewModel _viewModel;

    public CustomerDetailPage() : this(App.Services.GetRequiredService<CustomerDetailViewModel>())
    {
    }

    public CustomerDetailPage(CustomerDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Customer", out var customerObject) && customerObject is LocalCustomer customer)
        {
            _viewModel.Initialize(customer);
        }
        else
        {
            _viewModel.Initialize();
        }
    }
}
