using FinShark.DataAccess.Interfaces;
using FinShark.DataAccess.Interfaces.QueryParams;
using FinShark.DataAccess.Models;
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
    [HttpGet("query")]
    public async Task<IActionResult> Get(
        [FromQuery] FilterParam filter,
        [FromQuery] OrderParam order,
        [FromQuery] PageParam page)
    {
        try
        {
            var stocks = await _unitOfWork.StockRepository.GetAsync(
                filter: filter.IsSet() ? filter : null,
                order: order.IsSet() ? order : null,
                page: page.IsSet() ? page : null,
                includeProperties: $"{nameof(Stock.Comments)}");

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
