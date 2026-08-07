using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using CommsManager.Maui.Data.Models;
using CommsManager.Maui.ViewModels;

namespace CommsManager.Maui.Views;

public partial class ArtistDetailPage : ContentPage, IQueryAttributable
{
    private readonly ArtistDetailViewModel _viewModel;

    public ArtistDetailPage() : this(App.Services.GetRequiredService<ArtistDetailViewModel>())
    {
    }

    public ArtistDetailPage(ArtistDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Artist", out var artistObject) && artistObject is LocalArtistProfile artist)
        {
            _viewModel.Initialize(artist);
        }
        else
        {
            _viewModel.Initialize();
        }
    }
}
