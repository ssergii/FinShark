using System.Linq.Expressions;
using FinShark.DataAccess.Interfaces;
using FinShark.DataAccess.Models;
using FinShark.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Mappers;

namespace FinShark.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    #region fields
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockMapper _mapper;
    #endregion

    #region constructors
    public StockController(
        IUnitOfWork unitOfWork,
        IStockMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    #endregion

    #region end points
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var stocks = await _unitOfWork.StockRepository.GetAsync($"{nameof(Stock.Comments)}");
            var stockReadCollection = _mapper.ToStockReadCollection(stocks);

            return Ok(stockReadCollection);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        try
        {
            var stock = await _unitOfWork.StockRepository
                .GetSingleByAsync(x => x.Id == id, $"{nameof(Stock.Comments)}");
            if (stock == null)
                return NotFound();

            var stockRead = _mapper.ToStockRead(stock);

            return Ok(stockRead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("query")]
    public async Task<IActionResult> GetBy(
        [FromQuery] FilterParam filter,
        [FromQuery] OrderParam order,
        [FromQuery] PageParam page)
    {
        try
        {
            // var expression = filter.IsValid<Stock>() ? x => filter.Contains<Stock>(x) : x => true;
            Expression<Func<Stock, bool>>? expression = x => filter.Contains(x);
            Expression<Func<Stock, string>>? keySelector = x => order.Order<Stock>();

            var stocks = await _unitOfWork.StockRepository.GetByAsync(
                expression: x => filter.Contains(x),
                keySelector : x => order.Order<Stock>(),
                pageNumber: page.Number,
                pageSize: page.Size,
                includeProperties: $"{nameof(Stock.Comments)}");

            // var stocks = await _unitOfWork.StockRepository.GetAsync(
            // includeProperties: $"{nameof(Stock.Comments)}");

            var stockReadCollection = _mapper.ToStockReadCollection(stocks); // stocks.ToStockReadCollection();

            return Ok(stockReadCollection);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockCreate dto)
    {
        if (dto == null)
            return BadRequest("Stock data is null.");

        try
        {
            var stock = _mapper.ToStock(dto);
            if (stock == null)
                return BadRequest("Invalid stock data.");

            await _unitOfWork.StockRepository.CreateAsync(stock);

            var stockRead = _mapper.ToStockRead(stock);
            return CreatedAtAction(nameof(Get), new { id = stock.Id }, stockRead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StockUpdate dto)
    {
        if (dto == null)
            return BadRequest("Stock data is null.");

        try
        {
            var stock = await _unitOfWork.StockRepository.GetSingleByAsync(x => x.Id == id);
            if (stock == null)
                return NotFound();

            _mapper.Update(stock, dto);
            await _unitOfWork.StockRepository.UpdateAsync();

            var stockRead = _mapper.ToStockRead(stock);
            return Ok(stockRead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var deleteResult = await _unitOfWork.StockRepository.DeleteAsync(x => x.Id == id);
            if (!deleteResult)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    #endregion
}
