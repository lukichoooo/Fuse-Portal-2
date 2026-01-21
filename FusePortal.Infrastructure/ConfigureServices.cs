using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FusePortal.Infrastructure.Repo;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Infrastructure.EventDispatcher;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;
using FusePortal.Infrastructure.Settings.LLM;
using StackExchange.Redis;
using FusePortal.Infrastructure.Settings.Cache;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.Implementation;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Implementation;
using FusePortal.Application.Interfaces.Services;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Chat;
using FusePortal.Infrastructure.Services.SignalR;
using System.Text.Json;
using FusePortal.Infrastructure.Data.Cache;
using FusePortal.Infrastructure.Services.FileProcessor.Implementatoin;
using FusePortal.Infrastructure.Services.FileProcessor.Interfaces;
using FusePortal.Infrastructure.Services.FileProcessor;
using FusePortal.Infrastructure.Settings.File;
using FusePortal.Application.UseCases.Academic.Exams.Services;
using FusePortal.Infrastructure.Services.ExamGenerator;
using FusePortal.Domain.Entities.Academic.ExamAggregate;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Application.Interfaces.Services.File;
using FusePortal.Application.Interfaces.Services.PortalTransfer;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Portal;

namespace FusePortal.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            // EF
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("AppDbContext")));

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(
                _ => ConnectionMultiplexer.Connect(
                    configuration.GetConnectionString("Redis")
                    ?? throw new InvalidOperationException("Redis Connection String Null")));

            services.AddScoped<ICache, RedisCache>();


            // SignalR
            services.AddSignalR();
            services.AddSingleton<IMessageStreamer, SignalRMessageStreamer>();



            // Json
            services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            });


            // Settings
            // jtwSettings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EncryptorSettings>(configuration.GetSection("EncryptorSettings"));
            // llmSettings
            services.Configure<LLMApiSettingKeys>(configuration.GetSection("LLMApiSettingKeys"));

            services.Configure<LLMApiSettings>("Parser",
                configuration.GetSection("LLMApiSettings:Chat"));
            services.Configure<LLMApiSettings>("Chat",
                configuration.GetSection("LLMApiSettings:Chat"));
            services.Configure<LLMApiSettings>("Exam",
                configuration.GetSection("LLMApiSettings:Exam"));

            services.Configure<LLMInputSettings>(configuration.GetSection("LLMInputSettings"));
            // redisSettings
            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
            // fileSettings
            services.Configure<FileProcessingSettings>(configuration.GetSection("FileProcessingSettings"));
            services.Configure<TesseractSettings>(configuration.GetSection("TesseractSettings"));


            // auth
            services.AddHttpContextAccessor();
            services.AddScoped<IUserSecurityService, UserSecurityService>();
            services.AddScoped<IIdentityProvider, IdentityProvider>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEncryptor, Encryptor>();


            // repo
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUniRepo, UniRepo>();
            services.AddScoped<IChatRepo, ChatRepo>();
            services.AddScoped<IExamRepo, ExamRepo>();
            services.AddScoped<ISubjectRepo, SubjectRepo>();


            // events
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


            // LLM
            services.AddScoped<IChatMetadataService, ChatMetadataService>();
            services.AddScoped<ILLMApiSettingsChooser, LLMApiSettingsChooser>();
            services.AddScoped<ILLMInputGenerator, LLMInputGenerator>();
            services.AddSingleton<IPromptProvider, FilePromptProvider>(); // -- singleton
            // lmStudioLLM
            services.AddScoped<ILLMMessageService, LMStudioMessageService>();
            services.AddScoped<ILLMApiResponseStreamReader, LMStudioApiResponseStreamer>();
            services.AddScoped<ILMStudioApi, LMStudioApi>();
            services.AddScoped<ILMStudioMapper, LMStudioMapper>();
            services.AddScoped<IPortalTransferService, LMStudioPortalTransferService>();

            // File
            services.AddScoped<IFileParser, FileParser>();
            services.AddScoped<IFileProcessor, FileProcessingService>();

            // exam
            services.AddScoped<IExamService, LMStudioExamService>();


            // Ocr
            services.AddSingleton<IOcrService, PaddleOcrService>(); // -- singleton

            // Http Clients
            services.AddHttpClient<LMStudioApi>();


            return services;
        }
    }
}
