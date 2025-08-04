using FinShark.DataAccess.Models;
using FinShark.WebApi.DTOs;

namespace FinShark.WebApi.Mappers;

public static class UserMapper
{
    public static User ToUser(this UserCreate dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password
            // PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) // Hashing the password
        };

        return user;
    }

    public static UserRead ToUserRead(this User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return new UserRead
        {
            Name = user.Name,
            Email = user.Email,
            Token = user.Token
        };
    }
}
