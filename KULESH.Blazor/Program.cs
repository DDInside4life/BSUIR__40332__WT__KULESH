using KULESH.Blazor.Components;
using KULESH.Blazor.Services;
using KULESH.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ITeamService<FootballTeam>, ApiTeamService>(client =>
{
    client.BaseAddress = new Uri(builder
        .Configuration["ApiBaseUrl"] ?? "https://localhost:7002/");
});

builder.Services.AddServerSideBlazor();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
