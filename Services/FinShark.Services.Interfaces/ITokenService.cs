using FinShark.Services.Interfaces.Domain;

namespace FinShark.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
