using MyBudget.Core;
using MyBudget.Tests.Fakes;
using Xunit;

namespace MyBudget.Tests;

public class CustomAssignmentTests
{
    [Theory]
    [InlineData(10.999, 11.00)]
    [InlineData(1.234, 1.23)]
    public void RoundsAmount(decimal amount, decimal expected)
    {
        // Arrange / Act
        var expense = ExpenseFactory.CreateOneTime("Rounded amount", amount, ExpenseCategory.Other, default);

        // Assert
        Assert.Equal(expected, expense.Amount);
    }

    [Fact]
    public void SortsExpensesByDate()
    {
        // Arrange
        var repository = new ExpenseRepository(new InMemoryExpenseStore());
        repository.Add(new OneTimeExpense(Guid.NewGuid(), "Later", 1m, ExpenseCategory.Other, new DateOnly(2026, 2, 1)));
        repository.Add(new OneTimeExpense(Guid.NewGuid(), "Earlier", 1m, ExpenseCategory.Other, new DateOnly(2026, 1, 1)));

        // Act
        var expenses = repository.GetAll();

        // Assert
        Assert.Equal("Earlier", expenses[0].Description);
    }

    [Fact]
    public void KeepsBudgetOnTrackAtTenPercentLeft()
    {
        // Arrange
        var budget = new BudgetService();
        budget.SetMonthlyLimit(500m);

        // Act / Assert
        Assert.Equal(BudgetStatus.OnTrack, budget.Evaluate(450m));
    }
}
