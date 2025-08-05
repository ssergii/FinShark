using FakeItEasy;
using FinShark.DataAccess.Interfaces;
using FinShark.DataAccess.Models;
using FinShark.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Mappers;

namespace FinShark.Tests.ControllerTests;

public class StockControllerTest
{
    #region fields
    private readonly IUnitOfWork _unitOfWork;
    private readonly StockController _controller;
    #endregion

    #region constructor
    public StockControllerTest()
    {
        // Dependency injection for the controller
        _unitOfWork = A.Fake<IUnitOfWork>();

        // SUT - system under test
        _controller = new StockController(_unitOfWork);
    }
    #endregion

    #region tests
    [Fact]
    public async Task StockController_Get_AllStocks_ReturnsOk()
    {
        // Arrange
        var stocks = A.Fake<ICollection<Stock>>();
        A.CallTo(() => _unitOfWork.StockRepository.GetAsync($"{nameof(Stock.Comments)}"))
            .Returns(Task.FromResult(stocks));

        // Act
        var result = await _controller.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeEquivalentTo(stocks.ToStockReadCollection());
    }

    [Fact]
    public async Task StockController_Get_SingleStockById_ReturnsOk()
    {
        // Arrange
        var id = 1;
        var stock = A.Fake<Stock>();
        A.CallTo(() => _unitOfWork.StockRepository.GetSingleByAsync(x => x.Id == id, $"{nameof(Stock.Comments)}"))
            .Returns(Task.FromResult(stock));
        // A.CallTo(() => stock.ToStockRead())
        //     .Returns(new StockRead { Id = id });

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.Value.Should().BeOfType<StockRead>();

        var stockRead = okResult.Value as StockRead;
        stockRead.Id.Should().Be(id);
    }
    #endregion
}
