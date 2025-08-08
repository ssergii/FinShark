namespace FinShark.DataAccess.Interfaces.QueryParams;

public class OrderParam : ParamBase
{
    public string? OrderBy { get; set; }
    public bool IsDescending { get; set; }

    public override bool IsSet() => !string.IsNullOrEmpty(OrderBy);
}
