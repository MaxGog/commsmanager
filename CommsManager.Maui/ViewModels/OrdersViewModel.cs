using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;
using CommsManager.Core.Enums;
using CommsManager.Maui.Interfaces;

namespace CommsManager.Maui.ViewModels;

public partial class OrdersViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private List<LocalOrder> _orders = [];

    [ObservableProperty]
    private List<LocalCustomer> _customers = [];

    [ObservableProperty]
    private List<LocalArtistProfile> _artists = [];

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedStatus = "All";

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    public List<string> StatusOptions { get; } =
    [
        "All",
        "New",
        "InProgress",
        "Pending",
        "Completed",
        "Cancelled"
    ];

    public OrdersViewModel(DatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;

        LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
        AddOrderCommand = new AsyncRelayCommand(AddOrderAsync);
        DeleteOrderCommand = new AsyncRelayCommand<LocalOrder>(DeleteOrderAsync);
        EditOrderCommand = new AsyncRelayCommand<LocalOrder>(EditOrderAsync);
        //FilterOrdersCommand = new AsyncRelayCommand(FilterOrdersAsync);
    }

    public IAsyncRelayCommand LoadOrdersCommand { get; }
    public IAsyncRelayCommand AddOrderCommand { get; }
    public IAsyncRelayCommand<LocalOrder> DeleteOrderCommand { get; }
    public IAsyncRelayCommand<LocalOrder> EditOrderCommand { get; }
    public IAsyncRelayCommand FilterOrdersCommand { get; }

    private async Task LoadOrdersAsync()
    {
        IsLoading = true;

        try
        {
            var allOrders = await _databaseService.GetOrdersAsync();

            Customers = await _databaseService.GetCustomersAsync();
            Artists = await _databaseService.GetArtistProfilesAsync();

            ApplyFilters(allOrders);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyFilters(List<LocalOrder> allOrders)
    {
        var filtered = allOrders.AsEnumerable();

        if (SelectedStatus != "All")
        {
            filtered = filtered.Where(o => o.Status == SelectedStatus);
        }

        filtered = filtered.Where(o => o.Deadline.Date >= SelectedDate.Date);

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(o =>
                o.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                o.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true);
        }

        Orders = filtered.ToList();
    }

    private async Task AddOrderAsync()
    {
        await Shell.Current.GoToAsync("OrderDetailPage");
    }

    private async Task EditOrderAsync(LocalOrder? order)
    {
        if (order == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "Order", order }
        };

        await Shell.Current.GoToAsync("OrderDetailPage", parameters);
    }

    private async Task DeleteOrderAsync(LocalOrder? order)
    {
        if (order == null) return;

        bool confirmed = await _dialogService.ShowConfirmationAsync(
            "Удаление заказа",
            $"Вы уверены, что хотите удалить заказ '{order.Title}'?",
            "Удалить",
            "Отмена");

        if (confirmed)
        {
            await _databaseService.DeleteOrderAsync(order);
            await LoadOrdersAsync();
            await _dialogService.ShowToastAsync("Заказ удалён");
        }
    }

    [RelayCommand]
    private void ApplyFilters()
    {
        LoadOrdersCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SearchText = string.Empty;
        SelectedStatus = "All";
        SelectedDate = DateTime.Today;
        LoadOrdersCommand.ExecuteAsync(null);
    }
}