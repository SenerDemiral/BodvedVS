using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace BodvedVS.DataLibrary;

public static class CascadingValueSource
{
	public static CascadingValueSource<T> CreateNotifying<T>(T value, bool isFixed = false) where T : INotifyPropertyChanged
	{
		var source = new CascadingValueSource<T>(value, isFixed);

		value.PropertyChanged += (sender, args) => source.NotifyChangedAsync();

		return source;
	}
}
