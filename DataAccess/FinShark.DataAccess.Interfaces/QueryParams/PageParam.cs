namespace FinShark.DataAccess.Interfaces.QueryParams;

public class PageParam : ParamBase
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public override bool IsSet() => PageNumber > 0;
}
