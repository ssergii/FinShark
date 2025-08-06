using FinShark.DataAccess.Models;
using WebApi.DTOs;

namespace WebApi.Mappers;

public interface IStockMapper
{
    StockRead? ToStockRead(Stock stock);
    IEnumerable<StockRead> ToStockReadCollection(IEnumerable<Stock> stocks);
    Stock? ToStock(StockCreate stockCreate);
    Stock? ToStock(StockUpdate stockUpdate);
    void Update(Stock stock, StockUpdate stockUpdate);
}
