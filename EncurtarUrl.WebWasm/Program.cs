using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EncurtarUrl.WebWasm;
using MudBlazor.Services;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.WebWasm.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackEndUrl = builder.Configuration.GetValue<string>("BackEndUrl") ?? "/api";

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services
    .AddHttpClient(Configuration.HttpClientName, opt => { 
        var baseUrl = string.IsNullOrEmpty(Configuration.BackEndUrl) ? "/api" : Configuration.BackEndUrl;
        if (baseUrl.StartsWith("/"))
        {
            // Para URLs relativas, usar o BaseAddress do host atual
            opt.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), baseUrl);
        }
        else
        {
            // Para URLs absolutas
            opt.BaseAddress = new Uri(baseUrl);
        }
    });

builder.Services.AddTransient<IUrlHandler, UrlHandler>();
await builder.Build().RunAsync();
