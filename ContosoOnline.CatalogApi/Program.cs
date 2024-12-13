using ContosoOnline.CatalogApi;
using ContosoOnline.CatalogApi.Data;
using ContosoOnline.CatalogApi.Services;
using ContosoOnline.Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDbContext") ?? throw new InvalidOperationException("Connection string 'CatalogDbContext' not found.")));
builder.Services.AddSingleton<DatabaseSeeder<CatalogDbContext>, BogusFakeCatalogDatabaseSeeder>();
builder.Services.AddHostedService<DatabaseInitializer<CatalogDbContext>>();
builder.EnrichSqlServerDbContext<CatalogDbContext>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapProductEndpoints();

app.Run();

