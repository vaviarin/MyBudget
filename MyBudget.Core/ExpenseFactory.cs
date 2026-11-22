namespace MyBudget.Core;

public static class ExpenseFactory
{
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0m || amount > 1_000_000m)
            throw new InvalidExpenseException("Amount must be greater than 0 and no more than 1,000,000.");
        return decimal.Round(amount, 2, MidpointRounding.ToEven);
    }

    public static OneTimeExpense CreateOneTime(string description, decimal amount, ExpenseCategory category, DateOnly date)
    {
        description = CleanDescription(description);
        return new OneTimeExpense(Guid.NewGuid(), description, ValidateAmount(amount), category, date);
    }

    public static RecurringExpense CreateRecurring(string description, decimal amount, ExpenseCategory category, DateOnly date, int timesPerMonth)
    {
        description = CleanDescription(description);
        if (timesPerMonth < 1)
            throw new InvalidExpenseException("Times per month must be at least 1.");
        return new RecurringExpense(Guid.NewGuid(), description, ValidateAmount(amount), category, date, timesPerMonth);
    }

    private static string CleanDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidExpenseException("Description cannot be blank.");
        return description.Trim();
    }
}
