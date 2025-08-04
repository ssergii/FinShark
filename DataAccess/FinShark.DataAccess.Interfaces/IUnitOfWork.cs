using System;

namespace FinShark.DataAccess.Interfaces;

public interface IUnitOfWork
{
    public ICommentRepository CommentRepository { get; }
    public IStockRepository StockRepository { get; }
}
