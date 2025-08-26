using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tokero.Models;

namespace Tokero.ViewModels;

public partial class CoinSelectionViewModel : INotifyPropertyChanged
{
    private string _searchText = string.Empty;
    private ObservableCollection<CoinSelectionItem> _filteredCoins;
    private ObservableCollection<CoinSelectionItem> _selectedCoins;
    private Popup? _popup;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if(_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                OnSearchTextChanged();
            }
        }
    }

    public ObservableCollection<CoinSelectionItem> FilteredCoins
    {
        get => _filteredCoins;
        set
        {
            if(_filteredCoins != value)
            {
                _filteredCoins = value;
                OnPropertyChanged();
            }
        }
    }

    public List<string> AllCoins { get; set; }

    public ObservableCollection<CoinSelectionItem> SelectedCoins
    {
        get => _selectedCoins;
        set
        {
            if(_selectedCoins != value)
            {
                _selectedCoins = value;
                OnPropertyChanged();
            }
        }
    }

    public Action<List<string>>? OnSelectionChanged { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged ([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public CoinSelectionViewModel (List<string> coins, List<string> selectedCoins, Action<List<string>>? onSelectionChanged = null)
    {
        AllCoins = coins ?? new List<string>();

        var selectedItems = new List<CoinSelectionItem>();
        if(selectedCoins != null)
        {
            foreach(var coin in selectedCoins)
            {
                selectedItems.Add(new CoinSelectionItem(coin, true, OnCoinSelectionChanged));
            }
        }
        SelectedCoins = new ObservableCollection<CoinSelectionItem>(selectedItems);

        OnSelectionChanged = onSelectionChanged;

        _filteredCoins = new ObservableCollection<CoinSelectionItem>();
        foreach(var coin in AllCoins)
        {
            var isSelected = selectedCoins?.Contains(coin) ?? false;
            _filteredCoins.Add(new CoinSelectionItem(coin, isSelected, OnCoinSelectionChanged));
        }
    }

    public void SetPopup (Popup popup)
    {
        _popup = popup;
    }

    private void OnSearchTextChanged ()
    {
        FilterCoins();
    }

    private void FilterCoins ()
    {
        if(string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredCoins.Clear();
            foreach(var coin in AllCoins)
            {
                var isSelected = SelectedCoins.Any(sc => sc.Name == coin);
                FilteredCoins.Add(new CoinSelectionItem(coin, isSelected, OnCoinSelectionChanged));
            }
        }
        else
        {
            var filtered = AllCoins.Where(c => c.Contains(SearchText.ToUpper())).ToList();
            FilteredCoins.Clear();
            foreach(var coin in filtered)
            {
                var isSelected = SelectedCoins.Any(sc => sc.Name == coin);
                FilteredCoins.Add(new CoinSelectionItem(coin, isSelected, OnCoinSelectionChanged));
            }
        }
    }

    private void OnCoinSelectionChanged (CoinSelectionItem coinItem, bool isSelected)
    {
        if(coinItem == null || string.IsNullOrEmpty(coinItem.Name)) return;

        if(isSelected)
        {
            if(!SelectedCoins.Any(sc => sc.Name == coinItem.Name))
            {
                var newSelectedItem = new CoinSelectionItem(coinItem.Name, true, OnCoinSelectionChanged);
                SelectedCoins.Add(newSelectedItem);
            }
        }
        else
        {
            var existingSelected = SelectedCoins.FirstOrDefault(sc => sc.Name == coinItem.Name);
            if(existingSelected != null)
            {
                SelectedCoins.Remove(existingSelected);
            }
        }

        var selectedCoinNames = SelectedCoins.Select(sc => sc.Name).ToList();
        OnSelectionChanged?.Invoke(selectedCoinNames);
    }

    [RelayCommand]
    private async Task Close ()
    {
        if(_popup != null)
        {
            await _popup.CloseAsync();
        }
    }

    [RelayCommand]
    private async Task Done ()
    {
        if(_popup != null)
        {
            await _popup.CloseAsync();
        }
    }
}