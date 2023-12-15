namespace BodvedVS.Components;

public interface IBeniSilcs
{
    int MaxActConnCnt { get; set; }
    Action? OnChange { get; set; }
}

public class BeniSilcs : IBeniSilcs
{
    public Action? OnChange { get; set; }
    public int MaxActConnCnt { get; set; } = 0;
}
