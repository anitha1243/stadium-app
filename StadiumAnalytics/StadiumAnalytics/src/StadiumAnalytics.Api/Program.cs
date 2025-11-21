using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StadiumAnalytics.Api.Data;
using StadiumAnalytics.Api.Events;
using StadiumAnalytics.Api.Repositories;
using StadiumAnalytics.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core (SQLite file in project folder)
builder.Services.AddDbContext<StadiumContext>(options =>
    options.UseSqlite("Data Source=stadium.db"));

// Register application services
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddHostedService<EventSimulator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StadiumAnalytics.Api.Data.StadiumContext>();
    db.Database.EnsureCreated();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();