using BodvedVS;
using BodvedVS.Components;
using BodvedVS.Components.Comps;
using BodvedVS.DataLibrary;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateSlimBuilder(args);   ///////////CreateSlimBuilder scoped css development da gormuyor publish de sorun yok////////////////////

builder.Configuration.AddJsonFile("C:\\AspNetConfig\\BodvedVS.json",
                       optional: true,
                       reloadOnChange: true);

//builder.Services.AddHostedService<HostApplicationLifetimeEventsHostedService>();    // Start/Stop bilmek için deneme

// Add services to the container.
//builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.EnableDetailedErrors = false;
    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.MaximumParallelInvocationsPerClient = 1;
    options.MaximumReceiveMessageSize = 32 * 1024;
    options.StreamBufferCapacity = 10;
    if (options?.SupportedProtocols is not null)
    {
        foreach (var protocol in options.SupportedProtocols)
            Console.WriteLine($"SignalR supports {protocol} protocol.");
    }
});

builder.Services.AddSingleton<IDataAccess, FBDataAccess>();
builder.Services.AddSingleton<IAllUsrs, AllUsrs>();
builder.Services.AddHostedService<TimedHostedService>();

#region RootLevel CascadingValue Deneme
// Deneme
builder.Services.AddCascadingValue(s => CascadingValueSource.CreateNotifying(new Usr()));
// required files: Usr.cs DTO, CascadingValueSource.cs
// usage in interactive Page/Comp
// [CascadingParameter] public required Usr Usr {get; set;}
// <p> @Usr.Clicks </p>
// binding value directly
// <button @onclick="IncrementCount">Increment</button>
// <input type="number" title="Clicks" @bind-value:event="oninput" @bind-value="Usr.Clicks">
// void IncrementCount() => Usr.Clicks++;
#endregion

builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true); // iFrame ile embed edilebilir
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.MapGet("/{*rest}", (string rest) => Results.Redirect("/counter"));
//app.MapGet("/{*rest}", (string rest) => $"Routing to {rest}");

app.MapGet("/usrstat", (IAllUsrs usrs) =>
{
    var au = $"ActiveUserCount: {usrs.GetActUsrs()}";

	return au;
});

app.MapGet("/logout", (HttpContext context) =>
{
	var usrTkn = "g" + Guid.NewGuid().ToString("N"); // Guest
	CookieOptions co = new CookieOptions();
	co.Secure = false;
	co.HttpOnly = true;
	co.SameSite = SameSiteMode.Strict;
	//co.MaxAge = TimeSpan.FromHours(1);	// Bitis suresi belirtilmediginde sessionCookie oluyor
	context.Response.Cookies.Append("bodved", usrTkn, co);

	return Results.Redirect("/");
});

app.MapGet("/apict", async (IDataAccess db) => {

    var ct = (await db.LoadDataAsync<APIct, dynamic>("select * from API_CT", new { })).ToList();
    foreach (var r in ct)
    {
        r.Oyuncular = (await db.LoadDataAsync<APIctp, dynamic>("select * from API_CTP(@CTId)", new { CTId = r.Id })).ToList();
    }
    return ct;
});

app.MapGet("/apipp", async (IDataAccess db) =>

    (await db.LoadDataAsync<APIpp, dynamic>("select * from API_PP", new { })).ToList()
);


// ScopedContainer not working
//app.MapGet("/can", (ISingletonContainer sc) => new RazorComponentResult<Szn>(new { ad = sc.ActConnCnt }));

//Rendering Blazor components to a string
//https://andrewlock.net/exploring-the-dotnet-8-preview-rendering-blazor-components-to-a-string/

app.Run();

