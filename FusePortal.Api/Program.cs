using FusePortal.Api.Settings;
using FusePortal.Application;
using FusePortal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.Configure<ControllerSettings>(
        builder.Configuration.GetSection("ControllerSettings"));

var app = builder.Build();

app.MapControllers();

// app.UseHttpsRedirection();

app.Run();

