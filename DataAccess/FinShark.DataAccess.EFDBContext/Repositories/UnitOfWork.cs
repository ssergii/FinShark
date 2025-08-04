using FinShark.DataAccess.EFDBContext.Repositories;
using FinShark.DataAccess.Interfaces;

namespace FinShark.DataAccess.EFDBContext;

public class UnitOfWork : IUnitOfWork
{
    #region fields
    private readonly AppDBContext _dbContext;
    #endregion

    #region constructor
    public UnitOfWork(AppDBContext dbContext)
    {
        _dbContext = dbContext;

        CommentRepository = new CommentRepository(_dbContext);
        StockRepository = new StockRepository(_dbContext);
    }
    #endregion

    #region properties
    public ICommentRepository CommentRepository { get; }
    public IStockRepository StockRepository { get; }
    #endregion
}
