// =====================================================================
//  MyBudget — Assignment 2 entry point (composition root).
//
//  >>> YOU WRITE THE DEPENDENCY-INJECTION WIRING HERE (Module 8). <<<
//
//  The ConsoleApp UI is provided and depends only on the service
//  abstractions. Register your implementations with the container so that
//  ConsoleApp can be resolved. See the "Build specification" in the brief.
// =====================================================================
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBudget.App;
using MyBudget.Core;
using MyBudget.Data;

var builder = Host.CreateApplicationBuilder(args);

string filePath = Path.Combine(AppContext.BaseDirectory, "expenses.json");

// These singletons share one in-memory repository and budget state for the
// lifetime of this console process; the file store is stateless.
builder.Services.AddSingleton<IExpenseStore>(_ => new JsonExpenseStore(filePath));
builder.Services.AddSingleton<IExpenseRepository, ExpenseRepository>();
builder.Services.AddSingleton<IBudgetService, BudgetService>();
builder.Services.AddSingleton<ConsoleApp>();

using IHost host = builder.Build();

host.Services.GetRequiredService<ConsoleApp>().Run();
