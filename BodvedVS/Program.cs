using BodvedVS;
using BodvedVS.Components;
using BodvedVS.Components.Comps;
using BodvedVS.DataLibrary;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);   ///////////CreateSlimBuilder scoped css development da gormuyor publish de sorun yok////////////////////

builder.Configuration.AddJsonFile("C:\\AspNetConfig\\BodvedVS.json",
                       optional: true,
                       reloadOnChange: true);

//builder.Services.AddHostedService<HostApplicationLifetimeEventsHostedService>();    // Start/Stop bilmek i�in deneme

// Add services to the container.
//builder.Services.AddRazorComponents()
//.AddInteractiveServerComponents();

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

//builder.Services.AddMudServices(config =>
//{
//    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
//    config.SnackbarConfiguration.PreventDuplicates = false;
//    config.SnackbarConfiguration.NewestOnTop = true;
//    config.SnackbarConfiguration.ShowCloseIcon = true;
//    config.SnackbarConfiguration.VisibleStateDuration = 10000;
//    config.SnackbarConfiguration.HideTransitionDuration = 500;
//    config.SnackbarConfiguration.ShowTransitionDuration = 500;
//    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
//});

builder.Services.AddSingleton<ISingletonContainer, SingletonContainer>();
builder.Services.AddScoped<IScopedContainer, ScopedContainer>();
builder.Services.AddSingleton<IDataAccess, FBDataAccess>();

//builder.Services.AddMudBlazorSnackbar();
//builder.Services.AddBlazoredModal();

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

//app.MapGet("/{*rest}", (string rest) => Results.Redirect("/counter"));
//app.MapGet("/{*rest}", (string rest) => $"Routing to {rest}");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.Use((context, next) =>
//{
//    return next(context);
//});

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

