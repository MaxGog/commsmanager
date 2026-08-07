using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using CommsManager.Core.Models;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;
using CommsManager.Maui.Interfaces;

namespace CommsManager.Maui.ViewModels;

public partial class CustomerDetailViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private LocalCustomer _customer = new();

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private bool _isActive = true;

    [ObservableProperty]
    private ObservableCollection<Phones> _phones = new();

    [ObservableProperty]
    private bool _isNewCustomer = true;

    public CustomerDetailViewModel(DatabaseService databaseService, IDialogService dialogService)
    {
        _databaseService = databaseService;
        _dialogService = dialogService;

        SaveCommand = new AsyncRelayCommand(SaveCustomerAsync);
        CancelCommand = new AsyncRelayCommand(CancelAsync);
        AddPhoneCommand = new RelayCommand(AddPhone);
        RemovePhoneCommand = new RelayCommand<Phones>(RemovePhone);
    }

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
    public IRelayCommand AddPhoneCommand { get; }
    public IRelayCommand<Phones> RemovePhoneCommand { get; }

    public void Initialize(LocalCustomer? customer = null)
    {
        if (customer is null)
        {
            Customer = new LocalCustomer
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true
            };
            IsNewCustomer = true;
        }
        else
        {
            Customer = customer;
            IsNewCustomer = false;
        }

        Name = Customer.Name;
        Description = Customer.Description;
        IsActive = Customer.IsActive;
        Phones = new ObservableCollection<Phones>(Customer.Phones);
    }

    private async Task SaveCustomerAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await _dialogService.ShowAlertAsync("Ошибка", "Имя клиента обязательно", "OK");
            return;
        }

        Customer.Name = Name;
        Customer.Description = Description;
        Customer.IsActive = IsActive;
        Customer.Phones = Phones.ToList();
        Customer.UpdatedDate = DateTime.UtcNow;

        try
        {
            await _databaseService.SaveCustomerAsync(Customer);
            await _dialogService.ShowToastAsync("Клиент сохранён");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Ошибка", $"Не удалось сохранить клиента: {ex.Message}", "OK");
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

    private void AddPhone()
    {
        Phones.Add(new Phones { NumberPhone = string.Empty, TypePhone = "Mobile", Description = string.Empty });
    }

    private void RemovePhone(Phones? phone)
    {
        if (phone == null)
            return;

        Phones.Remove(phone);
    }
}
