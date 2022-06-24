using Microsoft.EntityFrameworkCore;
using TvScraperService.Core.Abstractions;
using TvScraperService.Core.Services;
using TvScraperService.Infrastructure;
using TvScraperService.Infrastructure.Repository;
using TvScraperService.Integration;
using TvScraperService.Integration.Mappings;
using TvScraperService.Integration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITvShowRepository, TvShowRepository>();
builder.Services.AddScoped<ITvMazeScraperService, TvMazeScraperService>();
builder.Services.AddSingleton<ITvMazeIntegrationService, TvMazeIntegrationService>();
builder.Services.AddDbContext<TvScraperDbContext>(options =>
    options.UseSqlServer(builder.Configuration["DbConnectionString"]));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(c => c.AddProfile<Mapping>(), typeof(TvMazeMarker));
builder.Services.AddHostedService<TvMazeIntegrationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<TvScraperDbContext>();
    dataContext.Database.Migrate();
}
app.MapControllers();

app.Run();
