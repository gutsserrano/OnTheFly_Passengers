using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnTheFly.AddressAPI.Data;
using OnTheFly.AddressAPI.PostalServices;
using OnTheFly.AddressAPI.PostalServices.Abstract;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OnTheFlyAddressAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnTheFlyAddressAPIContext") ?? throw new InvalidOperationException("Connection string 'OnTheFlyAddressAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSingleton<IPostalAddressService, ViaCepService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
