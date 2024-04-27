namespace BodvedVS.DataLibrary;

public class News
{
    public event Action<NewsModel>? OnChange;
    public void NotifyNewsChanged(NewsModel nm) => OnChange?.Invoke(nm);
}

public class NewsModel
{
    public int FrmId { get; set; }
    public int YpnId { get; set; }
}

/*

program.cs:
-----------
builder.Services.AddSingleton<News>();  // Global

NewsListener.razor:
--------------------
@rendermode @(new InteractiveServerRenderMode(prerender: false))
@inject News News
@implements IDisposable

<h3>NewsListener DENEME</h3>

@code {
	protected override void OnInitialized()
	{
		News.OnChange += NewsChanged;   // Listen
	}

	public void NewsChanged(NewsModel news)  // Handler
	{
		if (news.FrmId == 1234)
		{
			InvokeAsync(StateHasChanged);
		}
	}

	public void Dispose()
	{
		News.OnChange -= NewsChanged;
	}
}

NewsChanger.razor:
--------------------
@rendermode @(new InteractiveServerRenderMode(prerender: false))
@inject News News

<h3>NewsChanger DENEME</h3>

@code {
	private void PublishNews()
	{
		NewsModel news = new()
			{
				FrmId = 101,
				YpnId = 5,
			};
		News.NotifyNewsChanged(news);   // Something changed, emit news
	}
}

*/