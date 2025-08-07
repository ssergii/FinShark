namespace FinShark.DataAccess.Models;

public class Portfolio
{
    public string UserId { get; set; }
    public User? User { get; set; }
    public int StockId { get; set; }
    public Stock? Stock { get; set; }
}
