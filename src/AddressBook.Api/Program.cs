using System.Reflection;
using FluentValidation;
using WilderMinds.MinimalApiDiscovery;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<BookContext>();
builder.Services.AddTransient<BookEntryFaker>();
builder.Services.AddTransient<AddressFaker>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<IBookRepository, BookRepository>();

builder.Services.AddMapsterMaps();

builder.Services.AddOutputCache();

builder.Services.AddCors();

// Aspire Wiring
builder.AddServiceDefaults();
builder.AddRedisOutputCache("theCache");
builder.AddSqlServerDbContext<BookContext>("theDb");

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors(cfg => cfg.AllowAnyOrigin());

// Aspire endpoints (healthchecks, etc.)
app.MapDefaultEndpoints();

app.MapApis();
//app.MapFallbackToFile("/index.html");

app.Run();

