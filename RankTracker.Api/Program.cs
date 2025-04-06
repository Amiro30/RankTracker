using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RankTracker.Core;
using RankTracker.Core.Factories;
using RankTracker.Core.Parsers;
using RankTracker.Core.Services;
using RankTracker.DataAccess.Context;
using RankTracker.DataAccess.Repository;
using RankTracker.Infrastructure.Options;


var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ApplyInitialMigration(app);

ConfigurePipeline(app);

app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers()
        .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

    ConfigureSwagger(services);
    ConfigureCors(services);

    services.AddLogging(logging =>
    {
        logging.AddConsole().SetMinimumLevel(LogLevel.Information);
    });

    services.AddScoped<IHtmlParserFactory, HtmlParserFactory>();
    services.AddScoped<ISearchService, SearchService>();
    services.AddTransient<GoogleRegexHtmlParser>();
    services.AddTransient<BingHtmlParser>();

    services.Configure<SearchOptions>(configuration.GetSection("SearchOptions"));

    services.AddDbContext<RankTrackerDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ISearchLogRepository, SearchLogRepository>();
}


void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        // Uncomment if you want Swagger in dev mode
        // app.UseSwagger();
        // app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rank Tracker API", Version = "v1" });
    });
}

void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}

void ApplyInitialMigration(WebApplication webApplication)
{
    using (var scope = webApplication.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<RankTrackerDbContext>();
        dbContext.Database.Migrate();
    }
}

