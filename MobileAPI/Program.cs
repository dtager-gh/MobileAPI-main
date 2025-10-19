using Logto.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MobileAPI.Attributes;
using MobileAPI.Authentication;
using MobileAPI.Data;
using MobileAPI.Models;
using MobileAPI.Repos;
using MobileAPI.Services;
using MobileAPI.Swagger;
using Serilog;
using System;
using System.Reflection;

namespace MobileAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                
            );

            String connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString)
            );

            // Add services to the container.
            builder.Services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            builder.Services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddStackExchangeRedisCache(options =>
                options.Configuration = builder.Configuration.GetConnectionString("Cache"));

            builder.Services.AddControllers();

            /* Temporarily commented out for Jenkins testing
            builder.Services.AddLogtoAuthentication(options =>
            {
                options.Endpoint = builder.Configuration["Logto:Endpoint"]!;
                options.AppId = builder.Configuration["Logto:AppId"]!;
                options.AppSecret = builder.Configuration["Logto:AppSecret"];
            });
            */

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.EnableAnnotations();

                options.ParameterFilter<SortColumnFilter>();
                options.ParameterFilter<SortOrderFilter>();

                // Add an option to add ApiKey and value 
                options.AddSecurityDefinition(builder.Configuration.GetSection("ApiKey")["Header"], new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = builder.Configuration.GetSection("ApiKey")["Header"],
                    In = ParameterLocation.Header,
                    Description = "API Key Authentication"
                });

                // Apply the scheme to all operations(global requirement)
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = builder.Configuration.GetSection("ApiKey")["Header"]
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.OperationFilter<AuthRequirementFilter>();
                options.DocumentFilter<CustomDocumentFilter>();
                options.SchemaFilter<CustomKeyValueFilter>();
            });



            builder.Services.AddScoped<IRepo<BusinessOffice>, BusinessOfficeRepo>(); 
            builder.Services.AddScoped<IRepo<CafeteriaMenu>, CafeteriaMenuRepo>();
            builder.Services.AddScoped<IRepo<Campus>, CampusRepo>();
            builder.Services.AddScoped<IRepo<Library>, LibraryRepo>();
            builder.Services.AddScoped<IRepo<Registrar>, RegistrarRepo>();
            builder.Services.AddScoped<IRepo<TutoringCenter>, TutoringCenterRepo>();
            builder.Services.AddScoped<IRepo<Announcement>, AnnouncementRepo>();
            builder.Services.AddScoped<IRepo<SchoolEvent>, SchoolEventRepo>();
            builder.Services.AddScoped<IRepo<SchoolNews>, NewsRepo>();
            builder.Services.AddScoped<IRepo<School>, SchoolRepo>();
            builder.Services.AddScoped<IRepo<SecurityAlert>, SecurityAlertRepo>();
            builder.Services.AddScoped<IRepo<Security>, SecurityRepo>();
            builder.Services.AddScoped<IRepo<TutoringCenter>, TutoringCenterRepo>();
            builder.Services.AddScoped<IRepo<UserPreference>, UserPreferencesRepo>();
            builder.Services.AddScoped<IRepo<UserProfile>, UserProfileRepo>();

            builder.Services.AddScoped<CafeteriaSpecialService>();
            builder.Services.AddScoped<CafeteriaMenuService>();
            builder.Services.AddScoped<CampusService>();
            builder.Services.AddScoped<BusinessOfficeService>();
            builder.Services.AddScoped<LibraryService>();
            builder.Services.AddScoped<RegistrarService>();
            builder.Services.AddScoped<TutoringCenterService>();
            builder.Services.AddScoped<AnnouncementService>();
            builder.Services.AddScoped<SchoolEventService>();
            builder.Services.AddScoped<NewsService>();
            builder.Services.AddScoped<SchoolService>();
            builder.Services.AddScoped<SecurityAlertService>();
            builder.Services.AddScoped<SecurityService>();
            builder.Services.AddScoped<TutoringCenterService>();
            builder.Services.AddScoped<UserPreferencesService>();
            builder.Services.AddScoped<UserProfileService>();

            // These allow validating endpoints with [ApiKey]
            builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("ApiKey"));
            builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
            builder.Services.AddScoped<ApiKeyAuthorizationFilter>();

            var app = builder.Build();

            // Automatically apply EF Core migrations at startup
            using (var scope = app.Services.CreateScope())
            {
                IServiceProvider serviceProvider = scope.ServiceProvider;

                try
                {
                    ApplicationDbContext dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex.Message);
                }
            }

            // ngrok fix
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Adding security headers
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Frame-Options", "sameorigin");
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                // context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'nonce-23a98b38c'");
                context.Response.Headers.Append("Referrer-Policy", "strict-origin");
                await next();
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
