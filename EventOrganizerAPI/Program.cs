using EventOrganizerAPI.Persistance;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<EventOrganizerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventOrganizerDbConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
