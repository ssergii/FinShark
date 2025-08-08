namespace FinShark.DataAccess.Interfaces.QueryParams;

public class FilterParam : ParamBase
{
    public string? FilterBy { get; set; }
    public string? Value { get; set; }

    public override bool IsSet() => !string.IsNullOrEmpty(FilterBy);
}
