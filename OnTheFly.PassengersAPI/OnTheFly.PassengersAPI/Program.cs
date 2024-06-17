using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnTheFly.PassengersAPI.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OnTheFlyPassengersAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnTheFlyPassengersAPIContext") ?? throw new InvalidOperationException("Connection string 'OnTheFlyPassengersAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
