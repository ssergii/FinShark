using FinShark.DataAccess.EFDBContext.Base;
using FinShark.DataAccess.Interfaces;
using FinShark.DataAccess.Models;

namespace FinShark.DataAccess.EFDBContext.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    #region constructors
    public CommentRepository(AppDBContext dbContext) : base(dbContext) { }
    #endregion
}
