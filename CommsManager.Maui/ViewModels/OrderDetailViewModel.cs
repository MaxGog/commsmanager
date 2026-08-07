using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;
using CommsManager.Maui.Interfaces;

namespace CommsManager.Maui.ViewModels;

public partial class OrderDetailViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private LocalOrder _order = new();

    [ObservableProperty]
    private List<LocalCustomer> _customers = new();

    [ObservableProperty]
    private List<LocalArtistProfile> _artists = new();

    [ObservableProperty]
    private LocalCustomer? _selectedCustomer;

    [ObservableProperty]
    private LocalArtistProfile? _selectedArtist;

    [ObservableProperty]
    private bool _isNewOrder = true;

    [ObservableProperty]
    private bool _isSaving;

    public List<string> StatusOptions { get; } =
    [
        "New",
        "InProgress",
        "Pending",
        "Completed",
        "Cancelled"
    ];

    public OrderDetailViewModel(DatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;

        SaveCommand = new AsyncRelayCommand(SaveOrderAsync);
        CancelCommand = new AsyncRelayCommand(CancelAsync);
        ChangeStatusCommand = new AsyncRelayCommand<string>(ChangeStatusAsync);
        AddAttachmentCommand = new AsyncRelayCommand(AddAttachmentAsync);
    }

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
    public IAsyncRelayCommand<string> ChangeStatusCommand { get; }
    public IAsyncRelayCommand AddAttachmentCommand { get; }

    public void Initialize(LocalOrder? order = null)
    {
        if (order != null)
        {
            Order = order;
            IsNewOrder = false;
        }
        else
        {
            Order = new LocalOrder
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Deadline = DateTime.Today.AddDays(1),
                Status = "New",
                IsActive = true
            };
            IsNewOrder = true;
        }
    }

    public async Task LoadCustomersAndArtistsAsync()
    {
        Customers = await _databaseService.GetCustomersAsync();
        Artists = await _databaseService.GetArtistProfilesAsync();

        if (Order != null)
        {
            SelectedCustomer = Customers.FirstOrDefault(c => c.Id == Order.CustomerId);
            SelectedArtist = Artists.FirstOrDefault(a => a.Id == Order.ArtistId);
        }
    }

    partial void OnSelectedCustomerChanged(LocalCustomer? value)
    {
        if (value != null)
        {
            Order.CustomerId = value.Id;
            Order.Customer = value;
        }
    }

    partial void OnSelectedArtistChanged(LocalArtistProfile? value)
    {
        if (value != null)
        {
            Order.ArtistId = value.Id;
            Order.Artist = value;
        }
    }

    private async Task SaveOrderAsync()
    {
        if (string.IsNullOrWhiteSpace(Order.Title))
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Название заказа обязательно");
            return;
        }

        if (Order.PriceAmount <= 0)
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Цена должна быть больше 0");
            return;
        }

        if (Order.CustomerId == Guid.Empty)
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Выберите клиента");
            return;
        }

        if (Order.ArtistId == Guid.Empty)
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Выберите художника");
            return;
        }

        if (Order.Deadline <= DateTime.Now)
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Дедлайн должен быть в будущем");
            return;
        }

        IsSaving = true;

        try
        {
            await _databaseService.SaveOrderAsync(Order);
            await _dialogService.ShowToastAsync("Заказ сохранён");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Ошибка", $"Не удалось сохранить заказ: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
        }
    }

    private async Task CancelAsync()
    {
        bool confirm = await _dialogService.ShowConfirmationAsync(
            "Отмена",
            "Вы уверены, что хотите отменить изменения?",
            "Да",
            "Нет");

        if (confirm)
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task ChangeStatusAsync(string status)
    {
        Order.Status = status;

        if (status == "Completed" || status == "Cancelled")
        {
            Order.IsActive = false;
        }

        await _databaseService.SaveOrderAsync(Order);
        await _dialogService.ShowToastAsync($"Статус изменён на: {status}");
    }

    private async Task AddAttachmentAsync()
    {
        // TODO: Реализовать добавление вложений
        await _dialogService.ShowToastAsync("Функция добавления вложений в разработке");
    }
}