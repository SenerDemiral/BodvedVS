using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BodvedVS.DataLibrary;

public class Usr : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;
	int clicks;
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
