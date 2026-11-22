namespace MyBudget.Core;

public sealed class ExpenseRepository(IExpenseStore store) : IExpenseRepository
{
    private readonly List<Expense> _expenses = store.Load().ToList();

    public IReadOnlyList<Expense> GetAll() => _expenses.OrderBy(e => e.Date).ToList();

    public void Add(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);
        _expenses.Add(expense);
    }

    public decimal Total() => _expenses.Sum(e => e.MonthlyImpact);

    public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory() =>
        _expenses.GroupBy(e => e.Category).ToDictionary(g => g.Key, g => g.Sum(e => e.MonthlyImpact));

    public IReadOnlyList<Expense> InCategory(ExpenseCategory category) =>
        _expenses.Where(e => e.Category == category).OrderBy(e => e.Date).ToList();

    public void Save() => store.Save(_expenses);
}
