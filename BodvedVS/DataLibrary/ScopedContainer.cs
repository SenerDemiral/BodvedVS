namespace BodvedVS.DataLibrary;

public interface IScopedContainer
{
    List<int> AuthLigList { get; set; }
    bool IsOk { get; set; }
    int UsrId { get; set; }
}

public class ScopedContainer : IScopedContainer
{
    public int UsrId { get; set; } = 0;
    public bool IsOk { get; set; } = false;
    public List<int> AuthLigList { get; set; } = new();

}
