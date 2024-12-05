using CarRental;
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Custom DateOnly JSON converter
        options.SerializerSettings.Converters.Add(new JsonDateOnlyConverter());
    });

// Configure DbContext with SQL Server
builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper and the MappingProfile
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure JWT Authentication
string jwtKey = builder.Configuration["JwtSettings:Key"]
    ?? throw new ArgumentNullException("JwtSettings:Key", "JWT Key cannot be null or empty. Check your configuration.");
string jwtIssuer = builder.Configuration["JwtSettings:Issuer"]
    ?? throw new ArgumentNullException("JwtSettings:Issuer", "JWT Issuer cannot be null or empty.");
string jwtAudience = builder.Configuration["JwtSettings:Audience"]
    ?? throw new ArgumentNullException("JwtSettings:Audience", "JWT Audience cannot be null or empty.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Configure Swagger/OpenAPI for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Car Rental API",
        Version = "v1",
        Description = "API for car rental system, including reservation and payment processing"
    });

    // Add JWT support in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token as 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register repository services for Dependency Injection (DI)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAdminReportRepository, AdminReportRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental API v1");
        c.RoutePrefix = "swagger"; // Set Swagger UI as the root
    });
}
app.UseHttpsRedirection(); // Enforce HTTPS redirection
app.UseCors(builder =>
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyMethod()
           .AllowAnyHeader());


app.UseHttpsRedirection(); // Enforce HTTPS redirection

// Exception Handling Middleware
app.UseExceptionHandler(config =>
{
    config.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            var response = new
            {
                Error = exception.Message,
                Type = exception.GetType().Name
            };

            // Customize response based on exception type
            context.Response.StatusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                CustomUnauthorizedAccessException => StatusCodes.Status403Forbidden,
                DuplicateResourceException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

// Enable authentication and authorization
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Map API controllers

app.Run();

// Custom converter for DateOnly
public class JsonDateOnlyConverter : Newtonsoft.Json.JsonConverter<DateOnly>
{
    public override DateOnly ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
        {
            throw new JsonSerializationException("Invalid date format or null value for DateOnly field.");
        }

        if (DateOnly.TryParse(reader.Value.ToString(), out DateOnly date))
        {
            return date;
        }
        else
        {
            throw new JsonSerializationException($"Invalid date format: {reader.Value}");
        }
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, DateOnly value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString("yyyy-MM-dd"));
    }
}
