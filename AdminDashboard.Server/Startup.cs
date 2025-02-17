using System;
using AdminDashboard.Server.Helpers;
using AdminDashboard.Server.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;

namespace AdminDashboard.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<JwtTokenHttpMessageHandler>(); // Register JwtTokenHttpMessageHandler

            services.AddHttpClient("API", client =>
            {
                client.BaseAddress = new Uri("https://your-web-api-url/");
            })
            .AddHttpMessageHandler<JwtTokenHttpMessageHandler>();

            // Add Razor Pages and Blazor Server support
            services.AddRazorPages();
            services.AddServerSideBlazor();
            // Configure JWT Authentication for API calls


            // Configure Authentication and Authorization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/AccessDenied";
                });

            // Add MudBlazor services for Material Design components
            services.AddMudServices();

            // Register custom services
            services.AddHttpContextAccessor();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<AccountService>();
            services.AddScoped<VirtualAppointmentServices>();
            services.AddScoped<CustomAuthenticationStateProvider>();

            // Add HttpClient to the DI container
            services.AddHttpClient();

            services.AddBlazoredLocalStorage();


            // CORS Configuration for API interaction
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigin", builder =>
                    builder.WithOrigins("https://localhost:7161")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Optional: Configure proxy headers (useful for reverse proxies in production)
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                                           Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Add authentication and authorization middleware
            app.UseAuthentication();  // Enables authentication
            app.UseAuthorization();   // Enforces authorization based on authentication

            // Force HTTPS redirection and static file serving
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Enable CORS policy to handle cross-origin requests
            app.UseCors("AllowedOrigin");

            // Set up routing for Razor Pages, Blazor, and fallback page
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Map Blazor Hub for real-time communication
                endpoints.MapBlazorHub();

                // Map fallback page to handle default routing
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
