namespace BodvedVS.DataLibrary;

public interface ISingletonContainer
{
    string Name { get; set; }

    event Action? OnChange;
}

public class SingletonContainer : ISingletonContainer
{
    private string name;
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

}
