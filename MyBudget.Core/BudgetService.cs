namespace MyBudget.Core;

public sealed class BudgetService : IBudgetService
{
    public decimal MonthlyLimit { get; private set; }

    public void SetMonthlyLimit(decimal limit)
    {
        if (limit <= 0m)
            throw new InvalidExpenseException("Monthly limit must be greater than 0.");
        MonthlyLimit = decimal.Round(limit, 2, MidpointRounding.ToEven);
    }

    public decimal Remaining(decimal totalSpent) => MonthlyLimit - totalSpent;

    public BudgetStatus Evaluate(decimal totalSpent)
    {
        if (MonthlyLimit == 0m) return BudgetStatus.NotSet;
        decimal remaining = Remaining(totalSpent);
        if (remaining < 0m) return BudgetStatus.OverBudget;
        if (remaining < MonthlyLimit * 0.10m) return BudgetStatus.AlmostOut;
        return BudgetStatus.OnTrack;
    }
}
