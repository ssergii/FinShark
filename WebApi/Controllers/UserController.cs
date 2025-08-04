using FinShark.DataAccess.Interfaces;
using FinShark.WebApi.DTOs;
using FinShark.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    #region fields
    private readonly IUserRepository _userRepository;
    #endregion

    #region constructors
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    #endregion

    #region end points
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserCreate dto)
    {
        if (dto == null)
            return BadRequest("User data is null.");

        try
        {
            var user = dto.ToUser();
            await _userRepository.CreateAsync(user);

            var userRead = user.ToUserRead();
            return Ok(userRead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLogin dto)
    {
        if (dto == null || !ModelState.IsValid)
            return BadRequest("User data is null.");

        try
        {
            var user = await _userRepository.GetSingleByAsync(dto.Login, dto.Password);
            if (user is null)
                return Unauthorized("Invalid login or password.");

            var userRead = user.ToUserRead();

            return Ok(userRead);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    #endregion
}
