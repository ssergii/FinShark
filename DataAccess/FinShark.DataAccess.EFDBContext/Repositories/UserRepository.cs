using System.Linq.Expressions;
using FinShark.DataAccess.Interfaces;
using FinShark.DataAccess.Interfaces.QueryParams;
using FinShark.DataAccess.Models;
using FinShark.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinShark.DataAccess.EFDBContext.Repositories;

public class UserRepository : IUserRepository
{
    #region fiedls
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    // private readonly SignInManager<AppUser> _signInManager;
    #endregion

    #region constructors
    public UserRepository(
        UserManager<IdentityUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    #endregion

    #region methods
    public async Task<User?> GetSingleByAsync(string login, string password)
    {
        var identityUser = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == login || x.Email == login);
        if (identityUser is null)
            return null;

        var result = await _userManager.CheckPasswordAsync(identityUser, password);
        if (!result)
            return null;

        var token = _tokenService.GenerateToken(new Services.Interfaces.Domain.User(identityUser.UserName, identityUser.Email));
        var user = identityUser.ToUser(token);

        return user;
    }

    public async Task CreateAsync(User user)
    {
        var identityUser = user.ToIdentityUser();
        var result = await _userManager.CreateAsync(identityUser, user.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(". ", result.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(identityUser, "User");
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(". ", roleResult.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }

        var token = _tokenService.GenerateToken(new Services.Interfaces.Domain.User(user.Name, user.Email));
        user.Token = token;
    }

    public Task<bool> DeleteAsync(Expression<Func<User, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<User, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetSingleByAsync(Expression<Func<User, bool>> expression, string? includeProperties = null)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<User>> GetByAsync(FilterParam filter, OrderParam order, PageParam page, string? includeProperties = null)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<User>> GetAsync(FilterParam? filter = null, OrderParam? order = null, PageParam? page = null, string? includeProperties = null)
    {
        throw new NotImplementedException();
    }
    #endregion
}
