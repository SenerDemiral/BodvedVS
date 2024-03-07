using System;

namespace BodvedVS.DataLibrary;

public interface ISingletonContainer
{
    string Name { get; set; }
    int ConnCnt { get; set; }
    int ActConnCnt { get; set; }
    int MaxActConnCnt { get; set; }
    Dictionary<string, int> Conns { get; set; }

    event Action? OnChange;
    void AddMe(string usrGuid, int usrId);
    void OffMe(string usrGuid);
    int GetConnUsrId(string usrGuid);
}

public class SingletonContainer : ISingletonContainer
{
    public Dictionary<string, int> Conns { get; set; }
    private string? name;
    public int ConnCnt { get; set; }
    public int ActConnCnt { get; set; } = 0;
    public int MaxActConnCnt { get; set; } = 0;
    private IDataAccess _db;
    public event Action? OnChange;

    public SingletonContainer(IDataAccess db)
    {
        _db = db;
        this.Conns = new Dictionary<string, int>();
    }


    private void NotifyStateChanged() => OnChange?.Invoke();  //DynamicInvoke();
    public string Name
    {
        get => name;
        set
        {
            name = value;
            NotifyStateChanged();
        }
    }

    public void AddMe(string usrGuid, int usrId)
    {
        Conns.Add(usrGuid, usrId);
        ActConnCnt++;
        if (ActConnCnt > MaxActConnCnt)
        {
            MaxActConnCnt = ActConnCnt;
        }
        ConnCnt = _db.CONN_GIR(usrGuid, usrId);
        NotifyStateChanged();

    }
    public void OffMe(string usrGuid)
    {
        Conns.Remove(usrGuid);
        ActConnCnt--;
        _db.CONN_CIK(usrGuid);
        NotifyStateChanged();
    }

    public int GetConnUsrId(string usrGuid)
    {
        if(string.IsNullOrEmpty(usrGuid)) 
            return -2;
        if(Conns.TryGetValue(usrGuid, out int usrId))
        {
            return usrId;
        }
        return -1;
    }
}
