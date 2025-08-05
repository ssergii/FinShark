using FinShark.DataAccess.EFDBContext;
using FinShark.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Tests.RepositoryTests.DbContext;

public class TestDbContext
{
    public static async Task<AppDBContext> GetDbContextInMemory()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var dbContext = new AppDBContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        return dbContext;
    }

    public static async Task FillStocks(AppDBContext dbContext)
    {
        if (dbContext.Stocks.Any())
            return;

        var stocks = new List<Stock>
        {
            new Stock { Id = 1, Symbol = "TSL", CompanyName = "Tesla", Industry = "Car", Comments = new List<Comment>() },
            new Stock { Id = 2, Symbol = "MS", CompanyName = "Microsoft", Industry = "Softeare", Comments = new List<Comment>() },
            new Stock { Id = 3, Symbol = "DLL", CompanyName = "Dell", Industry = "Hardware", Comments = new List<Comment>() }
        };

        await dbContext.Stocks.AddRangeAsync(stocks);
        await dbContext.SaveChangesAsync();
    }
}
