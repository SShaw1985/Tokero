using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using System.Runtime.InteropServices;
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
           
            SearchResultsExpanded=true;
            InvestmentFrequency = InvestmentFrequency.Monthly;
            InvestmentFrequencies = new List<InvestmentFrequency> { InvestmentFrequency.Weekly, InvestmentFrequency.Monthly };
            UpdateDaysList();
        }

        public List<string> Coins { get; set; } = new();

        public List<string> SelectedCoins
        {
            get => _selectedCoins;
            set
            {
                _selectedCoins = value;
                CountAmounts = new List<CoinAmount>();
                foreach (var coin in _selectedCoins)
                {
                    CountAmounts.Add(new CoinAmount() { 
                        Symbol = coin, 
                        Amount=0 ,
                     StartDate = DateTime.Today.AddYears(-1)
                    });
                }
                ShowCoinAmounts = CountAmounts.Count>0;
                OnPropertyChanged(nameof(ShowCoinAmounts));
                OnPropertyChanged(nameof(SelectedCoins));
                OnPropertyChanged(nameof(CountAmounts));
            }
        }

        private List<string> _selectedCoins = new();
        public List<int> Days { get; set; } = new();
        public int SelectedDay { get; set; } = 15;
        private InvestmentFrequency _investmentFrequency = InvestmentFrequency.Monthly;
        public InvestmentFrequency InvestmentFrequency 
        { 
            get => _investmentFrequency;
            set
            {
                if (_investmentFrequency != value)
                {
                    _investmentFrequency = value;
                    UpdateDaysList();
                    OnPropertyChanged(nameof(InvestmentFrequency));
                    OnPropertyChanged(nameof(InvestmentFrequencyDescription));
                }
            }
        }
        public List<InvestmentFrequency> InvestmentFrequencies { get; set; } = new();
        public string InvestmentFrequencyDescription => InvestmentFrequency == InvestmentFrequency.Weekly ? "Weekly" : "Monthly";
        public List<DcaRecord> Records { get; set; } = new();
        public List<CoinAmount> CountAmounts { get; set; } = new();
        public decimal TotalInvested { get; set; }
        public decimal PortfolioValueToday { get; set; }
        public decimal OverallRoi { get; set; }
        public bool IsLoadingCryptocurrencies { get; set; }
        public bool SearchResultsExpanded { get; set; }
        public bool ShowCoinAmounts { get; set; }
        public Chart PortfolioValueChart { get; set; }
        public Chart RoiChart { get; set; }
        public bool IsInitialised { get; set; } = false;
        public List<ComparativePerformance> ComparativePerformances { get; set; } = new();
        public int UserPortfolioRank { get; set; }
        public string BestPerformingCoin { get; set; } = string.Empty;
        public decimal Top10AverageRoi { get; set; }
        public decimal BestPerformingRoi { get; set; }
        public decimal PerformanceVsBest { get; set; }

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
                        var amount = CountAmounts.FirstOrDefault(x => x.Symbol==coin)?.Amount;
                        var startDate = CountAmounts.FirstOrDefault(x => x.Symbol==coin)?.StartDate;
                        var result = await CalculateDcaAsync(coin, startDate.Value, DateTime.Today, SelectedDay, decimal.Parse(amount.ToString()), InvestmentFrequency);
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

                var earliestStartDate = CountAmounts.Min(x => x.StartDate);
                ComparativePerformances = await CalculateComparativePerformanceAsync(earliestStartDate, DateTime.Today, SelectedDay, InvestmentFrequency);

                if (ComparativePerformances.Count > 0)
                {
                    Top10AverageRoi = ComparativePerformances.Average(x => x.RoiPercentage);
                    BestPerformingCoin = ComparativePerformances.First().Symbol;
                    BestPerformingRoi = ComparativePerformances.First().RoiPercentage;
                    
                    var userRoi = OverallRoi * 100;
                    var userRank = ComparativePerformances.Count(x => x.RoiPercentage > userRoi) + 1;
                    UserPortfolioRank = userRank;
                    
                    PerformanceVsBest = BestPerformingRoi - userRoi;
                }

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
                Label = GetChartLabel(record.Date),
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
                Label = GetChartLabel(record.Date),
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

        private string GetChartLabel(DateTime date)
        {
            return InvestmentFrequency == InvestmentFrequency.Weekly 
                ? date.ToString("MMM dd") 
                : date.ToString("MMM yy");
        }

        private async Task<DcaResult> CalculateDcaAsync (string symbol, DateTime startDate, DateTime endDate, int dayOfPeriod, decimal amountPerPeriod, InvestmentFrequency frequency)
        {
            var resultRecords = new List<DcaRecord>();
            decimal totalInvestedLocal = 0m;
            decimal totalCoin = 0m;

            if (frequency == InvestmentFrequency.Monthly)
            {
                DateTime cursor = new DateTime(startDate.Year, startDate.Month, 1);
                DateTime endCursor = new DateTime(endDate.Year, endDate.Month, 1);

                while(cursor <= endCursor)
                {
                    int day = Math.Min(dayOfPeriod, DateTime.DaysInMonth(cursor.Year, cursor.Month));
                    DateTime investDate = new DateTime(cursor.Year, cursor.Month, day);
                    if(investDate > endDate)
                    {
                        break;
                    }

                    decimal priceOnDay = await priceService.GetHistoricalPriceAsync(symbol, investDate);
                    decimal coinAmount = amountPerPeriod / (priceOnDay == 0m ? 1m : priceOnDay);
                    totalInvestedLocal += amountPerPeriod;
                    totalCoin += amountPerPeriod;

                    decimal priceToday = await priceService.GetLatestPriceAsync(symbol);
                    decimal valueToday = coinAmount * priceToday;
                    decimal roi = amountPerPeriod == 0m ? 0m : (valueToday - amountPerPeriod) / amountPerPeriod;

                    resultRecords.Add(new DcaRecord
                    {
                        Date = investDate,
                        InvestedAmount = amountPerPeriod,
                        CoinAmount = coinAmount,
                        CoinSymbol = symbol,
                        ValueToday = valueToday,
                        Roi = roi
                    });

                    cursor = cursor.AddMonths(1);
                }
            }
            else
            {
                DateTime cursor = startDate.Date;
                
                while(cursor <= endDate)
                {
                    DateTime investDate = GetNextDayOfWeek(cursor, dayOfPeriod);
                    if(investDate > endDate)
                    {
                        break;
                    }

                    decimal priceOnDay = await priceService.GetHistoricalPriceAsync(symbol, investDate);
                    decimal coinAmount = amountPerPeriod / (priceOnDay == 0m ? 1m : priceOnDay);
                    totalInvestedLocal += amountPerPeriod;
                    totalCoin += amountPerPeriod;

                    decimal priceToday = await priceService.GetLatestPriceAsync(symbol);
                    decimal valueToday = coinAmount * priceToday;
                    decimal roi = amountPerPeriod == 0m ? 0m : (valueToday - amountPerPeriod) / amountPerPeriod;

                    resultRecords.Add(new DcaRecord
                    {
                        Date = investDate,
                        InvestedAmount = amountPerPeriod,
                        CoinAmount = coinAmount,
                        CoinSymbol = symbol,
                        ValueToday = valueToday,
                        Roi = roi
                    });

                    cursor = investDate.AddDays(7);
                }
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

        private async Task<List<ComparativePerformance>> CalculateComparativePerformanceAsync(DateTime startDate, DateTime endDate, int dayOfPeriod, InvestmentFrequency frequency)
        {
            var top10Coins = new List<string> { "BTC", "ETH", "SOL", "XRP", "ADA", "DOT", "LINK", "UNI", "AVAX", "MATIC" };
            var comparativeResults = new List<ComparativePerformance>();
            
            foreach (var coin in top10Coins)
            {
                try
                {
                    var result = await CalculateDcaAsync(coin, startDate, endDate, dayOfPeriod, 200m, frequency);
                    
                    comparativeResults.Add(new ComparativePerformance
                    {
                        Symbol = coin,
                        TotalInvested = result.TotalInvested,
                        CurrentValue = result.PortfolioValueToday,
                        Roi = result.OverallRoi,
                        RoiPercentage = result.OverallRoi * 100,
                        Rank = 0, 
                        IsInUserPortfolio = SelectedCoins.Contains(coin)
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to calculate comparative performance for {coin}: {ex.Message}");
                }
            }
            
            var sortedResults = comparativeResults.OrderByDescending(x => x.Roi).ToList();
            for (int i = 0; i < sortedResults.Count; i++)
            {
                sortedResults[i].Rank = i + 1;
            }
            
            return sortedResults;
        }

        private void UpdateDaysList()
        {
            if (InvestmentFrequency == InvestmentFrequency.Weekly)
            {
                Days = Enumerable.Range(0, 7).ToList();
                if (SelectedDay > 6) SelectedDay = 0; 
            }
            else
            {
                // For monthly: 1-28
                Days = Enumerable.Range(1, 28).ToList();
                if (SelectedDay < 1 || SelectedDay > 28) SelectedDay = 15;
            }
            OnPropertyChanged(nameof(Days));
        }

        private DateTime GetNextDayOfWeek(DateTime startDate, int dayOfWeek)
        {
            int currentDayOfWeek = (int)startDate.DayOfWeek;
            int daysToAdd = (dayOfWeek - currentDayOfWeek + 7) % 7;
            if (daysToAdd == 0 && startDate.Date < startDate.Date.AddDays(1))
            {
                daysToAdd = 7; 
            }
            return startDate.Date.AddDays(daysToAdd);
        }

        private SKColor GetRankColor(int rank)
        {
            return rank switch
            {
                1 => SKColor.Parse("#FFD700"), 
                2 => SKColor.Parse("#C0C0C0"), // Silver
                3 => SKColor.Parse("#CD7F32"), // Bronze
                <= 5 => SKColor.Parse("#4F46E5"), // Indigo for top 5
                <= 10 => SKColor.Parse("#6366F1"), // Purple for top 10
                _ => SKColor.Parse("#6B7280") // Gray for others
            };
        }
    }
}