using EncurtarUrl.api.Data;
using EncurtarUrl.api.EndPoints;
using EncurtarUrl.api.Handlers;
using EncurtarUrl.api.Services;
using EncurtarUrl.Core;
using EncurtarUrl.Core.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicione esta política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("wasm",
        policy => policy
            .WithOrigins([Configuration.BackEndUrl,
                          Configuration.FrontEndUrl,
                          "https://encurtador.brunoserver.ip-ddns.com/"]) // coloque a URL do seu frontend
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
    );
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("EncurtarUrlDb"));
Configuration.BackEndUrl = builder.Configuration.GetValue<string>("BackEndUrl") ?? string.Empty;
Configuration.FrontEndUrl = builder.Configuration.GetValue<string>("FrontEndUrl") ?? string.Empty;


builder.Services.AddTransient<IUrlHandler, Urlhandler>();
builder.Services.AddTransient<UrlShortenedService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(type => type.FullName); });

var app = builder.Build();

// Ative o CORS antes do UseAuthorization/UseEndpoints
app.UseCors("wasm");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapEndPoints();


app.Run();
