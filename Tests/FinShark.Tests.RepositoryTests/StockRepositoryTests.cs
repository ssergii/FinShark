using FinShark.DataAccess.EFDBContext;
using FinShark.DataAccess.EFDBContext.Repositories;
using FinShark.Tests.RepositoryTests.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Tests.RepositoryTests;

public class StockRepositoryTests
{
    // private readonly AppDBContext _dbContext;
    private readonly StockRepository _repository;

    public StockRepositoryTests()
    {
    }

    [Fact]
    public async Task StockRepository_CreateAsync_ShouldAddStock()
    {
        var dbContext = await TestDbContext.GetDbContextInMemory();
        await TestDbContext.FillStocks(dbContext);
    }

    

    
}
