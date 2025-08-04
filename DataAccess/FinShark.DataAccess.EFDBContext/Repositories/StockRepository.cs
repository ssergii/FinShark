using FinShark.DataAccess.EFDBContext.Base;
using FinShark.DataAccess.Interfaces;
using FinShark.DataAccess.Models;

namespace FinShark.DataAccess.EFDBContext.Repositories;

public class StockRepository : Repository<Stock>, IStockRepository
{
    #region constructors
    public StockRepository(AppDBContext dbContext) : base(dbContext) { }
    #endregion
}
