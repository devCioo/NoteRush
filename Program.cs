using BitzArt.Blazor.Auth.Server;
using BitzArt.Blazor.Cookies;
using MudBlazor.Services;
using NoteRush.Components;
using NoteRush.Data;
using NoteRush.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddLocalization();
builder.Services.AddDbContextFactory<NoteRushDbContext>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = builder.Configuration.GetSection("Localization:SupportedCultures").Get<string[]>();
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});
builder.AddBlazorCookies();
builder.AddBlazorAuth<NoteRushAuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapAuthEndpoints();

app.Run();
