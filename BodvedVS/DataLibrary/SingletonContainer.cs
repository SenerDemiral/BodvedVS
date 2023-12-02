namespace BodvedVS.DataLibrary;

public interface ISingletonContainer
{
    string Name { get; set; }
    int ConnCnt { get; set; }
    int ActConnCnt { get; set; }

    event Action? OnChange;
    void AddMe();
    void OffMe();
}

public class SingletonContainer(IDataAccess db) : ISingletonContainer
{
    private string name;
    public int ConnCnt { get; set; }
    public int ActConnCnt { get; set; } = 0;
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

    public void AddMe()
    {
        ActConnCnt++;
        ConnCnt = _db.CONN_INC_GET();
        NotifyStateChanged();
    }
    public void OffMe()
    {
        ActConnCnt--;
        NotifyStateChanged();
    }
}
