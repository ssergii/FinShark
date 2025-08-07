using System.Linq.Expressions;
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
    public async Task StockController_Get_ReturnsOk()
    {
        // arrange
        var prop = nameof(Stock.Comments);
        var stocks = A.Fake<ICollection<Stock>>();
        var stockReadCollection = A.Fake<IEnumerable<StockRead>>();

        A.CallTo(() => _unitOfWork.StockRepository.GetAsync(prop))
            .Returns(Task.FromResult(stocks));
        A.CallTo(() => _mapper.ToStockReadCollection(stocks))
            .Returns(stockReadCollection);

        // act
        var result = await _controller.Get();

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var readCollection = okResult.Value as IEnumerable<StockRead>;
        readCollection.Should().NotBeNull();
        readCollection.Should().BeEquivalentTo(stockReadCollection);
    }

    [Fact]
    public async Task StockController_Get_ById_ReturnsOk()
    {
        // arrange
        var id = 1;
        var prop = nameof(Stock.Comments);
        var stock = A.Fake<Stock>();
        var stockRead = A.Fake<StockRead>();

        A.CallTo(() => _unitOfWork.StockRepository.GetSingleByAsync(A<Expression<Func<Stock, bool>>>._, prop))
            .Returns(Task.FromResult(stock));
        A.CallTo(() => _mapper.ToStockRead(stock))
            .Returns(stockRead);

        // act
        var result = await _controller.Get(id);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var dtoRead = okResult.Value as StockRead;
        dtoRead.Should().BeEquivalentTo(stockRead);
    }

    [Fact]
    public async Task StockController_Create_RetutnActionResult()
    {
        // arrange
        var stockCreate = A.Fake<StockCreate>();
        var stock = A.Fake<Stock>();
        var stockRead = A.Fake<StockRead>();

        A.CallTo(() => _mapper.ToStock(stockCreate)).Returns(stock);
        A.CallTo(() => _unitOfWork.StockRepository.CreateAsync(stock)).Returns(Task.CompletedTask);
        A.CallTo(() => _mapper.ToStockRead(stock)).Returns(stockRead);

        // act
        var result = await _controller.Create(stockCreate);

        // assert
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().BeAssignableTo<StockRead>();

        var dtoRead = createdResult.Value as StockRead;
        dtoRead.Should().BeEquivalentTo(stockRead);
    }

    [Fact]
    public async Task StockController_Update_ReturnsOk()
    {
        // arange
        var id = 1;
        var prop = string.Empty;
        var stockUpdate = A.Fake<StockUpdate>();
        var stock = A.Fake<Stock>();
        var stockRead = A.Fake<StockRead>();

        A.CallTo(() => _unitOfWork.StockRepository.GetSingleByAsync(A<Expression<Func<Stock, bool>>>._, prop))
            .Returns(stock);
        A.CallTo(() => _mapper.Update(stock, stockUpdate));
        A.CallTo(() => _unitOfWork.StockRepository.UpdateAsync()).Returns(Task.CompletedTask);
        A.CallTo(() => _mapper.ToStockRead(stock)).Returns(stockRead);

        // act
        var result = await _controller.Update(id, stockUpdate);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var dtoRead = okResult.Value as StockRead;
        dtoRead.Should().NotBeNull();
        dtoRead.Should().BeEquivalentTo(stockRead);
    }

    [Fact]
    public async Task StockController_Delete_ReturnNoContent()
    {
        // arange
        var id = 1;
        var isDeleted = true;

        A.CallTo(() => _unitOfWork.StockRepository.DeleteAsync(A<Expression<Func<Stock, bool>>>._))
            .Returns(isDeleted);

        // act
        var result = await _controller.Delete(id);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task StockController_Delete_ReturnNotFound()
    {
        // arange
        var id = 1;
        var isDeleted = false;

         A.CallTo(() => _unitOfWork.StockRepository.DeleteAsync(A<Expression<Func<Stock, bool>>>._))
            .Returns(isDeleted);

        // act
        var result = await _controller.Delete(id);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }
    #endregion
}
