using FinShark.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Mappers;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    #region fields
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region constructors
    public CommentController(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;
    #endregion

    #region methods
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var comments = await _unitOfWork.CommentRepository.GetAsync();
            var commentReadCollection = comments.ToCommentReadCollection();

            return Ok(commentReadCollection);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        try
        {
            var comment = await _unitOfWork.CommentRepository.GetSingleByAsync(x => x.Id == id);
            if (comment == null)
                return NotFound();

            var commentRead = comment.ToCommentRead();

            return Ok(commentRead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CommentCreate dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
            {
                var stockExists = await _unitOfWork.StockRepository.ExistsAsync(x => x.Id == stockId);
                if (!stockExists)
                    return NotFound($"Stock with ID {stockId} does not exist.");

                var comment = dto.ToComment(stockId);
                if (comment == null)
                    return BadRequest("Invalid comment data.");

                await _unitOfWork.CommentRepository.CreateAsync(comment);

                var commentRead = comment.ToCommentRead();
                return CreatedAtAction(nameof(Get), new { id = commentRead.Id }, commentRead);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CommentUpdate dto)
    {
        if (dto == null)
            return BadRequest("Comment data is null.");

        try
        {
            var comment = await _unitOfWork.CommentRepository.GetSingleByAsync(x => x.Id == id);
            if (comment == null)
                return NotFound();

            if (!comment.Update(dto))
                return BadRequest("Invalid comment data.");

            await _unitOfWork.CommentRepository.UpdateAsync();

            var readDTO = comment.ToCommentRead();
            return Ok(readDTO);
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
            var deleted = await _unitOfWork.CommentRepository.DeleteAsync(x => x.Id == id);
            if (!deleted)
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
