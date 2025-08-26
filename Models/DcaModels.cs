namespace Tokero.Models
{
    public enum InvestmentFrequency
    {
        Weekly,
        Monthly
    }

    public class DcaRecord
    {
        public DateTime Date { get; set; }
        public string CoinSymbol { get; set; } = string.Empty;
        public decimal InvestedAmount { get; set; }
        public decimal CoinAmount { get; set; }
        public decimal ValueToday { get; set; }
        public decimal Roi { get; set; }
        public decimal PriceOnInvestmentDate { get; set; }
        public decimal CurrentPrice { get; set; }
    }

    public class DcaResult
    {
        public List<DcaRecord> Records { get; set; } = new();
        public decimal TotalInvested { get; set; }
        public decimal PortfolioValueToday { get; set; }
        public decimal OverallRoi { get; set; }
    }

    public class CoinAllocation
    {
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal AllocationPercentage { get; set; }
        public decimal MonthlyAmount { get; set; }
        public DateTime? CustomStartDate { get; set; }
    }

    public class PortfolioSummary
    {
        public List<CoinAllocation> Allocations { get; set; } = new();
        public decimal TotalMonthlyInvestment { get; set; }
        public InvestmentFrequency InvestmentFrequency { get; set; } = InvestmentFrequency.Monthly;
        public int InvestmentDayOfPeriod { get; set; } = 15; // Day of month or day of week
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ComparativePerformance
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal TotalInvested { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Roi { get; set; }
        public decimal RoiPercentage { get; set; }
        public int Rank { get; set; }
        public bool IsInUserPortfolio { get; set; }
    }

    public class CoinAmount
    {
        public string Symbol { get; set; }
        public double Amount { get; set; }
        public DateTime StartDate{ get; set; }
    }
}