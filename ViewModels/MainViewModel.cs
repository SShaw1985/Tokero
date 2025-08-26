using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using Tokero.Interfaces;
using Tokero.Models;

namespace Tokero.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IPriceService priceService;
        private readonly ICachingService cachingService;
        private readonly IAppPopupService appPopupService;
        private bool _cryptocurrenciesLoaded = false;

        public MainViewModel (IPriceService injectedPriceService, ICachingService injectedCachingService, IAppPopupService injectedAppPopup)
        {
            priceService = injectedPriceService ?? throw new ArgumentNullException(nameof(injectedPriceService));
            cachingService = injectedCachingService ?? throw new ArgumentNullException(nameof(injectedCachingService));
            appPopupService = injectedAppPopup?? throw new ArgumentNullException(nameof(injectedAppPopup));
            Coins = new List<string>();
            SelectedCoins = new List<string>();
            Days = Enumerable.Range(1, 28).ToList();
            SelectedDay = 15;
            StartDate = DateTime.Today.AddYears(-1);
            SearchResultsExpanded=true;
            MonthlyAmount = 200m;
        }

        public List<string> Coins { get; set; } = new();

        public List<string> SelectedCoins
        {
            get => _selectedCoins;
            set
            {
                _selectedCoins = value;
                OnPropertyChanged(nameof(SelectedCoins));
            }
        }

        private List<string> _selectedCoins = new();
        public List<int> Days { get; set; } = new();
        public int SelectedDay { get; set; } = 15;
        public DateTime StartDate { get; set; } = DateTime.Today.AddYears(-1);
        public decimal MonthlyAmount { get; set; } = 200m;
        public List<DcaRecord> Records { get; set; } = new();
        public decimal TotalInvested { get; set; }
        public decimal PortfolioValueToday { get; set; }
        public decimal OverallRoi { get; set; }
        public bool IsLoadingCryptocurrencies { get; set; }
        public bool SearchResultsExpanded { get; set; }

        public Chart PortfolioValueChart { get; set; }
        public Chart RoiChart { get; set; }
        public bool IsInitialised { get; set; } = false;

        public async Task InitializeAsync ()
        {
            if(!IsInitialised)
            {
                try
                {
                    if(!_cryptocurrenciesLoaded)
                    {
                        appPopupService.ShowLoading();
                        await Task.Delay(1000);
                        await LoadCryptocurrenciesAsync();
                        _cryptocurrenciesLoaded = true;
                    }
                    SearchResultsExpanded = true;
                    IsInitialised=true;
                }
                finally
                {
                    appPopupService.CloseLoading();
                }
            }
        }

        [RelayCommand]
        private async Task LoadCryptocurrenciesAsync ()
        {

            try
            {
                IsLoadingCryptocurrencies = true;

                var cryptocurrencies = await priceService.GetAllCryptocurrenciesAsync(50);
                var symbols = cryptocurrencies?.Select(c => c.Symbol).ToList() ?? new List<string>();

                Coins = symbols;
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load cryptocurrencies: {ex.Message}", "OK");
                Coins = new List<string> { "BTC", "ETH", "SOL", "XRP", "USDC" };
                SelectedCoins = new List<string> { Coins.First() };
            }
            finally
            {
                IsLoadingCryptocurrencies = false;
            }
        }


        [RelayCommand]
        private void ToggleCoinSelection (string coin)
        {
            if(string.IsNullOrEmpty(coin)) return;

            var currentSelection = new List<string>(SelectedCoins);
            if(currentSelection.Contains(coin))
            {
                currentSelection.Remove(coin);
            }
            else
            {
                currentSelection.Add(coin);
            }

            SelectedCoins = currentSelection;
        }

        [RelayCommand]
        private async Task ShowCoinSelection ()
        {
            try
            {
                await appPopupService.ShowCoinSelectionPopup(
                    Coins,
                    new List<string>(SelectedCoins),
                    (selectedCoins) =>
                    {
                        SelectedCoins = new List<string>(selectedCoins);
                    }
                );
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to show coin selection: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task Calculate ()
        {
            if(SelectedCoins == null || SelectedCoins.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("No Coins Selected", "Please select at least one cryptocurrency first.", "OK");
                return;
            }

            try
            {
                appPopupService.ShowLoading();
                var allRecords = new List<DcaRecord>();
                decimal totalInvestedLocal = 0m;
                decimal totalPortfolioValue = 0m;

                foreach(var coin in SelectedCoins)
                {
                    try
                    {
                        var result = await CalculateDcaAsync(coin, StartDate, DateTime.Today, SelectedDay, MonthlyAmount);
                        allRecords.AddRange(result.Records);
                        totalInvestedLocal += result.TotalInvested;
                        totalPortfolioValue += result.PortfolioValueToday;
                    }

                    catch(Exception ex)
                    {
                        appPopupService.CloseLoading();
                        await Application.Current.MainPage.DisplayAlert("Calculation Error",
                            $"Failed to calculate DCA for {coin}: {ex.Message}", "OK");
                        return;
                    }
                }

                Records = allRecords.OrderBy(r => r.Date).ToList();
                TotalInvested = totalInvestedLocal;
                PortfolioValueToday = totalPortfolioValue;
                OverallRoi = totalInvestedLocal == 0m ? 0m : (totalPortfolioValue - totalInvestedLocal) / totalInvestedLocal;

                GenerateCharts();
                SearchResultsExpanded=true;
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Calculation Error", $"Failed to complete DCA calculation: {ex.Message}", "OK");
            }
            finally
            {
                appPopupService.CloseLoading();
            }
        }

        private void GenerateCharts ()
        {
            if(Records == null || Records.Count == 0) return;

            var portfolioEntries = Records.Select((record, index) => new ChartEntry((float)record.ValueToday)
            {
                Label = record.Date.ToString("MMM yy"),
                ValueLabel = $"€{record.ValueToday:N0}",
                Color = SKColor.Parse("#6366F1"),
            }).ToArray().Take(6);

            PortfolioValueChart = new LineChart
            {
                Entries = portfolioEntries,
                LineMode = LineMode.Straight,
                LineSize = 3,
                PointMode = PointMode.Circle,
                PointSize = 6,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                LabelTextSize = 24,
                IsAnimated = true,
            };

            var roiEntries = Records.Select((record, index) => new ChartEntry((float)(record.Roi * 100))
            {
                Label = record.Date.ToString("MMM yy"),
                ValueLabel = $"{record.Roi:P1}",
                Color = record.Roi >= 0 ? SKColor.Parse("#10B981") : SKColor.Parse("#EF4444")
            }).ToArray().Take(6);

            RoiChart = new BarChart
            {
                Entries = roiEntries,
                BackgroundColor = SkiaSharp.SKColors.Transparent,
                LabelTextSize = 24,
                IsAnimated = true,
            };
        }

        private async Task<DcaResult> CalculateDcaAsync (string symbol, DateTime startDate, DateTime endDate, int dayOfMonth, decimal monthlyEur)
        {
            var resultRecords = new List<DcaRecord>();
            DateTime cursor = new DateTime(startDate.Year, startDate.Month, 1);
            DateTime endCursor = new DateTime(endDate.Year, endDate.Month, 1);
            decimal totalInvestedLocal = 0m;
            decimal totalCoin = 0m;

            while(cursor <= endCursor)
            {
                int day = Math.Min(dayOfMonth, DateTime.DaysInMonth(cursor.Year, cursor.Month));
                DateTime investDate = new DateTime(cursor.Year, cursor.Month, day);
                if(investDate > endDate)
                {
                    break;
                }

                decimal priceOnDay = await priceService.GetHistoricalPriceAsync(symbol, investDate);
                decimal coinAmount = monthlyEur / (priceOnDay == 0m ? 1m : priceOnDay);
                totalInvestedLocal += monthlyEur;
                totalCoin += coinAmount;

                decimal priceToday = await priceService.GetLatestPriceAsync(symbol);
                decimal valueToday = coinAmount * priceToday;
                decimal roi = monthlyEur == 0m ? 0m : (valueToday - monthlyEur) / monthlyEur;

                resultRecords.Add(new DcaRecord
                {
                    Date = investDate,
                    InvestedAmount = monthlyEur,
                    CoinAmount = coinAmount,
                    CoinSymbol = symbol,
                    ValueToday = valueToday,
                    Roi = roi
                });

                cursor = cursor.AddMonths(1);
            }

            decimal latestPrice = await priceService.GetLatestPriceAsync(symbol);
            return new DcaResult
            {
                Records = resultRecords,
                TotalInvested = totalInvestedLocal,
                PortfolioValueToday = totalCoin * latestPrice,
                OverallRoi = totalInvestedLocal == 0m ? 0m : ((totalCoin * latestPrice) - totalInvestedLocal) / totalInvestedLocal
            };
        }
    }
}