using FusePortal.Api.Settings;
using FusePortal.Application;
using FusePortal.Application.Common.Settings;
using FusePortal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);


// settings
builder.Services.Configure<ControllerSettings>(
        builder.Configuration.GetSection("ControllerSettings"));
builder.Services.Configure<ValidatorSettings>(
        builder.Configuration.GetSection("ValidatorSettings"));

var app = builder.Build();

app.MapControllers();

// app.UseHttpsRedirection();

app.Run();

