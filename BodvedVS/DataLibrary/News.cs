namespace BodvedVS.DataLibrary;

public class News
{
    public event Action<NewsModel>? OnChange;
    private void NotifyNewsChanged(NewsModel nm) => OnChange?.Invoke(nm);
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

SomeListener.razor:
--------------------
@implements IDisposable
@inject News News

@code {
    protected override void OnInitialized()
    {
        News.OnChange += NewsChanged;   // Listen
    }

    public void NewsChanged(News news)  // Handler
	{
		if (news.FrmId == appState.FrmId) 
		{
			InvokeAsync(StateHasChanged);
		}
	}

    public void Dispose()
    {
        News.OnChange -= NewsChanged;
    }
}


SomeProducer.razor:
--------------------
@implements IDisposable
@inject News News

<p>
    <button @onclick="PublishNews">
        Send news to listeners
    </button>
</p>

@code {

    private void PublishNews()
	{
		News news = new()
		{
			FrmId = 101,
			YpnId = 5,
		};
		News.NotifyNewsChanged(news);   // Something changed, emit news
	}
}

*/