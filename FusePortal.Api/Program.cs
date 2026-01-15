using FusePortal.Application;
using FusePortal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();

// app.UseHttpsRedirection();

app.Run();

