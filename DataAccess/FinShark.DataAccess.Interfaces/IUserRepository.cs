using FinShark.DataAccess.Interfaces.Base;
using FinShark.DataAccess.Models;

namespace FinShark.DataAccess.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetSingleByAsync(string login, string password);
}
