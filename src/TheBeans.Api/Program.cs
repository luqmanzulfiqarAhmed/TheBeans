

using Microsoft.EntityFrameworkCore;
using TheBeans.Api.Extensions;
using TheBeans.Application.Extensions;
using TheBeans.Infrastructure.Data;
using TheBeans.Infrastructure.Extensions;
 
var builder = WebApplication.CreateBuilder(args);

// Register API-specific services.
builder.Services.AddApiServices(); 


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Auto-migrate on startup
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
