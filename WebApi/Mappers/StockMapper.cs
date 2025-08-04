using FinShark.DataAccess.Models;
using WebApi.DTOs;

namespace WebApi.Mappers;

public static class StockMapper
{
    public static StockRead? ToStockRead(this Stock stock)
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

    public static IEnumerable<StockRead> ToStockReadCollection(this IEnumerable<Stock> stocks)
    {
        if (stocks is null || !stocks.Any())
            return Enumerable.Empty<StockRead>();

        var stockReadCollection = stocks.Select(stock => stock.ToStockRead());

        return stockReadCollection;
    }

    public static Stock? ToStock(this StockCreate stockCreate)
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

    public static Stock? ToStock(this StockUpdate stockUpdate)
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

    public static void Update(this Stock stock, StockUpdate stockUpdate)
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
