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
    private readonly IStockMapper _mapper;
    private readonly StockController _controller;
    #endregion

    #region constructor
    public StockControllerTest()
    {
        // Dependency injection for the controller
        _unitOfWork = A.Fake<IUnitOfWork>();
        _mapper = A.Fake<IStockMapper>();

        // SUT - system under test
        _controller = new StockController(_unitOfWork, _mapper);
    }
    #endregion

    #region tests
    [Fact]
    public async Task StockController_Get_AllStocks_ReturnsOk()
    {
        // Arrange
        var stocks = A.Fake<ICollection<Stock>>();
        var dtoReadCollection = A.Fake<IEnumerable<StockRead>>();

        A.CallTo(() => _unitOfWork.StockRepository.GetAsync($"{nameof(Stock.Comments)}"))
            .Returns(Task.FromResult(stocks));
        A.CallTo(() => _mapper.ToStockReadCollection(stocks))
            .Returns(dtoReadCollection);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var readCollection = okResult.Value as IEnumerable<StockRead>;
        readCollection.Should().NotBeNull();
        readCollection.Should().BeEquivalentTo(dtoReadCollection);
    }

    [Fact]
    public async Task StockController_Get_SingleStockById_ReturnsOk()
    {
        // Arrange
        var id = 1;
        var stock = A.Fake<Stock>();
        var dtoRead = A.Fake<StockRead>();

        A.CallTo(() => _unitOfWork.StockRepository.GetSingleByAsync(x => x.Id == id, $"{nameof(Stock.Comments)}"))
            .Returns(Task.FromResult(stock));
        A.CallTo(() => _mapper.ToStockRead(stock))
            .Returns(dtoRead);

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var stockRead = okResult.Value as StockRead;
        stockRead.Should().BeEquivalentTo(dtoRead);
    }

    [Fact]
    public async Task StockController_Create_RetutnActionResult()
    {
        // Arrange
        var dtoCreate = A.Fake<StockCreate>();
        var stock = A.Fake<Stock>();
        var dtoRead = A.Fake<StockRead>();

        A.CallTo(() => _mapper.ToStock(dtoCreate)).Returns(stock);
        A.CallTo(() => _unitOfWork.StockRepository.CreateAsync(stock)).Returns(Task.CompletedTask);
        A.CallTo(() => _mapper.ToStockRead(stock)).Returns(dtoRead);

        // Act
        var result = await _controller.Create(dtoCreate);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().BeAssignableTo<StockRead>();

        var stockRead = createdResult.Value as StockRead;
        stockRead.Should().BeEquivalentTo(dtoRead);
    }
    #endregion
}
