namespace BodvedVS.DataLibrary;

public interface IScopedContainer
{
    List<int> AuthLigList { get; set; }
    bool IsOk { get; set; }
    int UsrId { get; set; }

    bool CheckLogin(int userid, int password);
}

public class ScopedContainer : IScopedContainer
{
    public int UsrId { get; set; } = 0;
    public bool IsOk { get; set; } = false;
    public List<int> AuthLigList { get; set; } = new();
    public bool CheckLogin(int userid, int password)
    {
        AuthLigList.Clear();
        IsOk = false;
        if (userid > 0 && userid == -password)
        {
            UsrId = userid;
            IsOk = true;

            return true;
        }
        return false;
    }
}
