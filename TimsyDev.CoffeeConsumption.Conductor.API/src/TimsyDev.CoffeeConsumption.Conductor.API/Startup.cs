﻿using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using CoffeeConsumption.Conductor.API.Config;
using CoffeeConsumption.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using TimsyDev.CoffeeConsumption.Shared.Config;
using TimsyDev.CoffeeConsumption.Shared.Data;
using ILogger = Serilog.ILogger;

namespace TimsyDev.CoffeeConsumption.Conductor.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        private AWSOptions _awsOptions;
        private CoffeeConsumptionDynamoConfig _dynamoConfig;
        private readonly string _corsPolicy = "_allowTestAllOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            _awsOptions = Configuration.GetAWSOptions();
            _dynamoConfig = Configuration
                .GetSection("AWSDynamoDBConfig")
                .Get<CoffeeConsumptionDynamoConfig>() ?? throw new InvalidOperationException($"Error: Appsettings.EnvService.EnvironmentName.json Missing \"AWSDynamoDBConfig\" Section");


            services.AddDefaultAWSOptions(_awsOptions);
            services.AddSingleton<ICoffeeConsumptionDynamoConfig>(_dynamoConfig);
            services.AddAWSService<IAmazonDynamoDB>();

            services.AddSingleton<ICoffeeDrinkService, CoffeeDrinkService>();
            services.AddSingleton<ICoffeeDrinkerDataService, CoffeeDrinkDataService>();
            services.AddSingleton<IMockDataService, MockDataService>();

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter());

            loggerConfig.WriteTo.File(new RenderedCompactJsonFormatter(), "App_Data/log.txt", rollingInterval: RollingInterval.Day);

            ILogger logger = loggerConfig.CreateLogger();
            Log.Logger = logger;
            services.AddSingleton(logger);

            try
            {
                services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

                var appConfig = Configuration.GetSection("AppConfig").Get<AppConfig>();

                services.AddCors(options =>
                {
                    options.AddPolicy(_corsPolicy,
                        builder =>
                        {
                            //builder.WithOrigins(appConfig.WithOrigins)
                            //.AllowAnyMethod()
                            //.AllowAnyHeader();
                            builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
                });

                services.AddSwaggerGen();

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error Starting Application");
                throw;
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger, IOptionsSnapshot<AppConfig> appConfig)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
                        options.RoutePrefix = string.Empty;
                    });
                }

                app.UseCors(_corsPolicy);

                app.UseHttpsRedirection();

                app.UseRouting();


                //app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error With Application Configuration.");
                throw;
            }

        }
    }
}