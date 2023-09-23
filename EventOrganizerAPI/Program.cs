using EventOrganizerAPI.DbInitializer;
using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Mapper;
using EventOrganizerAPI.Middleware;
using EventOrganizerAPI.Persistance;
using EventOrganizerAPI.Services;
using EventOrganizerAPI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using EventOrganizerAPI.Authorization;
using FluentValidation;
using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Models.Validators;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddFluentValidationClientsideAdapters();

var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

builder.Services.AddDbContext<EventOrganizerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventOrganizerDbConnection"))
);

builder.Services.AddScoped<IEventOrganizerService, EventOrganizerService>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(EventMappingProfile));

builder.Services.AddScoped<IValidator<CreateEventDto>, CreateEventDtoValidator>();

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
