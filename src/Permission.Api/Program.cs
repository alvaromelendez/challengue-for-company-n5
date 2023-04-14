
using System.Reflection;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using Permissions.Api.Application.IntegrationEvents;
using Permissions.Api.Configs;
using Permissions.Api.HostedService;
using Permissions.Infrastructure;
using Permissions.Infrastructure.Services;
using Permissions.Infrastructure.Services.Blocks;
using Serilog;

namespace Permissions.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((hostContext, services, configuration) => {
                configuration.WriteTo.Console();
                configuration.Enrich.FromLogContext();
            });

            builder.Configuration.AddEnvironmentVariables();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //MetiatR DependencyInjection
            builder.Services.AddMediatR(config => {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            //builder.Services.AddSingleton<IHostedService, ApacheKafkaConsumerService>();
            builder.Services.AddHostedService<ApacheKafkaConsumerService>();
            builder.Services.AddScoped<IRequestIntegrationEventService,RequestIntegrationEventService>();
            builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());

            //Elasticsearch
            var _elasticConfig = builder.Configuration.GetSection(nameof(ElasticConfig)).Get<ElasticConfig>();
            builder.Services.AddSingleton<IElasticClient>((sp) =>
            {
                var pool = new SingleNodeConnectionPool(new Uri(_elasticConfig.Url));
                var settings = new ConnectionSettings(pool)
                    //.DefaultIndex("defaultindex")
                    .DefaultMappingFor<PermissionBlock>(m => m
                        .IndexName("permission_index").IdProperty(d => d.Id));

                settings.BasicAuthentication(_elasticConfig.User, _elasticConfig.Password);

                if (_elasticConfig.EnableLogs)
                    settings.DisableDirectStreaming();

                return new ElasticClient(settings);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
