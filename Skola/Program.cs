using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skola.API.Data;
using Skola.API.Services;


Console.WriteLine("Started Application");

var builder = WebApplication.CreateBuilder(args);

// Add cache services
builder.Services.AddMemoryCache();

// Register the DbContext with the appropriate connection string
builder.Services.AddDbContext<Skola24Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Skola24Connection")));

// Register ISchoolService and SchoolService
builder.Services.AddScoped<ISchoolService, SchoolService>();

// Register ISchoolService and SchoolService
builder.Services.AddScoped<IStudentService, StudentService>();

// Add controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddApplicationPart(typeof(Program).Assembly)
    .AddControllersAsServices();

// Add controllers with XML documentation
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly)
    .AddControllersAsServices();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Serve default files like index.html
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
