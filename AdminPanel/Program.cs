using AdminPanel.Components;
using AdminPanel.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddScoped<AccountServices>();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigin", builder =>
        builder.WithOrigins("https://localhost:7161") // The origin of your Blazor app
               .AllowAnyMethod()
               .AllowAnyHeader());
});

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

app.UseHttpsRedirection();

app.UseCors("AllowedOrigin");
app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
