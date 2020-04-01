using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        private const string BibleApiKeyKeyName = "BibleApi:Key";
        private const string BibleApiUrlKeyName = "BibleApi:Url";
        private const string DbConnectionStringName = "VersesDbConnectionString";
        private const string EnableCompressionKeyName = "EnableResponseCompression";
        private const string HealthCheckPath = "/hc";
        private const string SwaggerAPITitle = "My API";
        private const string SwaggerAPIVersion = "v1";
        private const string SwaggerDocPath = "/swagger/v1/swagger.json";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add EF Core services
            services.AddDbContext<VersesDbContext>(options =>
                options.UseSqlServer(Configuration[DbConnectionStringName], options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }));

            // Add verse lookup services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IVerseService, VerseService>();
            services.AddTransient<LookupServiceResolver>(serviceProvider => translation =>
            {
                return translation switch
                {
                    Translations.KJV => serviceProvider.GetRequiredService<BibleApiVerseLookupService>(),
                    Translations.ESV => serviceProvider.GetRequiredService<BibleApiVerseLookupService>(),
                    Translations.ASV => serviceProvider.GetRequiredService<BibleApiVerseLookupService>(),
                    Translations.RSV => serviceProvider.GetRequiredService<BibleApiVerseLookupService>(),
                    _ => serviceProvider.GetRequiredService<IVerseLookupService>()
                };
            });

            services.AddScoped<BibleApiVerseLookupService>();
            services.AddHttpClient(BibleApiVerseLookupService.BibleApiHttpClientName, client =>
            {
                client.BaseAddress = new Uri(Configuration[BibleApiUrlKeyName]);
                client.DefaultRequestHeaders.Add("api-key", Configuration[BibleApiKeyKeyName]);
            })
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(i * 2)))
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            // Add MVC services
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            // Add auth services, including JWT bearer scheme
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));

            // Add response compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            });

            // Add Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"{SwaggerAPIVersion}", new OpenApiInfo { Title = SwaggerAPITitle, Version = SwaggerAPIVersion });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[0]
                    }
                });
            });

            // Add health check services
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
                IdentityModelEventSource.ShowPII = true;

                app.UseDeveloperExceptionPage();

                // Allow calls from the Vue dev server
                app.UseCors(policy => policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:8080", "http://localhost:5000", "https://localhost:5001", "http://fcfverses.org", "https://fcfverses.org")
                    .AllowCredentials());
            }

            // app.UseHttpsRedirection();
            if (bool.TryParse(Configuration[EnableCompressionKeyName], out var useResponseCompression) && useResponseCompression)
            {
                app.UseResponseCompression();
            }

            // Serve static files (including the SPA app)
            app.UseStaticFiles();

            // Serve Swagger and Swagger UI endpoints
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerDocPath, $"{SwaggerAPITitle} {SwaggerAPIVersion}");
            });

            // Use endpoint routing with auth
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                // Route to MVC controllers
                endpoints.MapControllers();

                // Map to health check endpoints
                endpoints.MapHealthChecks(HealthCheckPath);

                // Defer to the SPA to route unrecognized paths
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
