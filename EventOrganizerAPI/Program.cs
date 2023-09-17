using EventOrganizerAPI.DbInitializer;
using EventOrganizerAPI.Mapper;
using EventOrganizerAPI.Middleware;
using EventOrganizerAPI.Persistance;
using EventOrganizerAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<EventOrganizerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventOrganizerDbConnection"))
);

builder.Services.AddScoped<IEventOrganizerService, EventOrganizerService>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(EventMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
