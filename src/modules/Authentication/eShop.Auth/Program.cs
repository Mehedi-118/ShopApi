using eShop.Auth.Helpers;
using eShop.Auth.Middleware;

using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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
builder.Services.RegisterServices(builder.Configuration);
var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration).CreateLogger();
builder.Logging.AddSerilog(logger, dispose: true);
builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();