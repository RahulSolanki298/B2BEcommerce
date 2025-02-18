using Core.Entities;
using Core.IRepository;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database Context Configuration
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Setup
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// JWT Authentication Setup
var key = Encoding.ASCII.GetBytes(builder.Configuration["Token:SecretKey"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.FromMinutes(5) // 5-minute clock skew tolerance
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.Headers.Add("Token-Error", "Authentication failed");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.Response.Headers.Add("Token-Error", "Authorization failed");
                return Task.CompletedTask;
            },
        };
    });

// Authorization Setup
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SuppliersOnly", policy => policy.RequireRole("Suppliers"));
    options.AddPolicy("BuyersOnly", policy => policy.RequireRole("Buyers"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
});

// Dependency Injection for Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IVirtualAppointmentRepository, VirtualAppointmentRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();

// Swagger and API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(builder.Configuration.GetValue<string>("Cors:AllowedOrigin") ?? "https://localhost:7161");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

// Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
