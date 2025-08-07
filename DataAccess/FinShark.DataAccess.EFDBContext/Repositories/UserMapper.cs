using FinShark.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace FinShark.DataAccess.EFDBContext.Repositories;

public static class UserMapper
{
    public static IdentityUser ToIdentityUser(this User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var identityUser = new IdentityUser
        {
            UserName = user.Name,
            Email = user.Email,
        };

        return identityUser;
    }

    public static User ToUser(this IdentityUser identityUser, string token)
    {
        if (identityUser is null)
            throw new ArgumentNullException(nameof(identityUser));

        var user = new User
        {
            Id = identityUser.Id,
            Name = identityUser.UserName,
            Email = identityUser.Email,
            Token = token
        };

        return user;
    }
}
