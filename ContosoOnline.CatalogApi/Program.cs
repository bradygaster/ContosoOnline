using ContosoOnline.CatalogApi;
using ContosoOnline.CatalogApi.Data;
using ContosoOnline.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDbContext") ?? throw new InvalidOperationException("Connection string 'CatalogDbContext' not found.")));
builder.Services.AddHostedService<DatabaseInitializer<CatalogDbContext>>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapProductEndpoints();

app.Run();

