using System.Text;

using eShop.Auth.Domain.Entities;
using eShop.Auth.Helpers;
using eShop.Auth.Infrastructure.DBContext;
using eShop.Auth.Infrastructure.Settings;
using eShop.Auth.Middleware;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
/*builder.Services.AddSwaggerGen();*/
var connectionString = builder.Configuration.GetConnectionString("eShop.Auth.SqlServer");
var a = typeof(IAuthInfrastructureAssemblyMarker).Assembly.FullName;
builder.Services.AddDbContextPool<AuthenticationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly(typeof(IAuthInfrastructureAssemblyMarker).Assembly.FullName);
        sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
    });
}, 2048);

builder.Services.AddDbContextPool<AuthenticationReadOnlyDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
    }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}, 2048);
ConfigureServices.RegisterServices(builder.Services, builder.Configuration);


builder.Services.AddIdentity<User, Role>(options =>
    {
        options.Password.RequiredLength = 6; // Customize as needed
        options.Lockout.MaxFailedAccessAttempts = 5; // Customize as needed
    })
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "eShop.Auth",
            Description = "An ASP.NET Core Web API for managing Authentication and Authorization",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact { Name = "Example Contact", Url = new Uri("https://example.com/contact") },
            License = new OpenApiLicense { Name = "Example License", Url = new Uri("https://example.com/license") }
        });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();