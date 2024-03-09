using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BodvedVS.DataLibrary;

public class Usr : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;
	int clicks;
    int id;
    public int Id
    {
        get => id;
        set
        {
            if (value != id)
            {
                id = value;
                OnPropertyChanged();
            }
        }
    }

    public string Ad { get; set; } = "Guest";
    public bool IsAdm { get; set; } = false;
    public bool IsTnm { get; set; } = false;
    public bool IsSnc { get; set; } = false;

    public int Clicks {
		get => clicks;
		set
		{
			if (value != clicks)
			{
				clicks = value;
				OnPropertyChanged();
			}
		}
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = default)
		=> PropertyChanged?.Invoke(this, new(propertyName));
}
