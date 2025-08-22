using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EncurtarUrl.WebWasm;
using MudBlazor.Services;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.WebWasm.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackEndUrl = builder.Configuration.GetValue<string>("BackEndUrl") ?? string.Empty;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services
    .AddHttpClient(Configuration.HttpClientName, client =>
    {
        client.BaseAddress = new Uri(Configuration.BackEndUrl);
    });

builder.Services.AddTransient<IUrlHandler, UrlHandler>();
await builder.Build().RunAsync();
