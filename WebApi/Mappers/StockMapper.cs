using FinShark.DataAccess.Models;
using WebApi.DTOs;

namespace WebApi.Mappers;

public class StockMapper : IStockMapper
{
    public StockRead? ToStockRead(Stock stock)
    {
        if (stock is null) return null;

        var stockRead = new StockRead
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            LastDiv = stock.LastDiv,
            Industry = stock.Industry,
            MarketCap = stock.MarketCap,
            Comments = stock.Comments.ToCommentReadCollection()
        };

        return stockRead;
    }

    public IEnumerable<StockRead> ToStockReadCollection(IEnumerable<Stock> stocks)
    {
        foreach (var stock in stocks)
            yield return ToStockRead(stock);
    }

    public Stock? ToStock(StockCreate stockCreate)
    {
        if (stockCreate is null) return null;

        var stock = new Stock
        {
            Symbol = stockCreate.Symbol,
            CompanyName = stockCreate.CompanyName,
            Purchase = stockCreate.Purchase,
            LastDiv = stockCreate.LastDiv,
            Industry = stockCreate.Industry,
            MarketCap = stockCreate.MarketCap
        };

        return stock;
    }

    public Stock? ToStock(StockUpdate stockUpdate)
    {
        if (stockUpdate is null) return null;

        var stock = new Stock
        {
            Symbol = stockUpdate.Symbol,
            CompanyName = stockUpdate.CompanyName,
            Purchase = stockUpdate.Purchase,
            LastDiv = stockUpdate.LastDiv,
            Industry = stockUpdate.Industry,
            MarketCap = stockUpdate.MarketCap
        };

        return stock;
    }

    public void Update(Stock stock, StockUpdate stockUpdate)
    {
        if (stock is null || stockUpdate is null) return;

        stock.Symbol = stockUpdate.Symbol;
        stock.CompanyName = stockUpdate.CompanyName;
        stock.Purchase = stockUpdate.Purchase;
        stock.LastDiv = stockUpdate.LastDiv;
        stock.Industry = stockUpdate.Industry;
        stock.MarketCap = stockUpdate.MarketCap;
    }
}
