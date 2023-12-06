using System;

namespace BodvedVS.DataLibrary;

public interface ISingletonContainer
{
    string Name { get; set; }
    int ConnCnt { get; set; }
    int ActConnCnt { get; set; }
    int MaxActConnCnt { get; set; }

    event Action? OnChange;
    void AddMe(string usrGuid, int usrId);
    void OffMe(string usrGuid);
}

public class SingletonContainer(IDataAccess db) : ISingletonContainer
{
    private string name;
    public int ConnCnt { get; set; }
    public int ActConnCnt { get; set; } = 0;
    public int MaxActConnCnt { get; set; } = 0;
    private IDataAccess _db = db;

    public event Action? OnChange;

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
        ActConnCnt--;
        _db.CONN_CIK(usrGuid);
        NotifyStateChanged();
    }
}
