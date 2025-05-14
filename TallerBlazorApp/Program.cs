using TallerBlazorApp.Components;
using TallerBlazorApp;
using TallerBlazorApp.Data;
using TallerBlazorApp.Data.Auth;
using TallerBlazorApp.Data.Services;
//using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Blazor
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddAuthorizationCore();

// Servicios personalizados
builder.Services.AddSingleton<StateContainer>();
builder.Services.AddSingleton<TokenContainer>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<PersonajeService>();
builder.Services.AddSingleton<Consumer>();
//builder.Services.AddBlazoredSessionStorage();

// Registro de AuthenticationStateProvider personalizado
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
