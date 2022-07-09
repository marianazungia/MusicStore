using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MusicStore.API.HealthChecks;
using MusicStore.API.Profiles;
using MusicStore.DataAccess;
using MusicStore.Entities.Configurations;
using MusicStore.Services;
using MusicStore.Services.Implementations;
using MusicStore.Services.Interfaces;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var corsConfiguration = "MusicStoreAPI";
/*
 * LEVELS
 * 1. INFORMATION
 * 2. WARNING
 * 3. ERROR
 * 4. FATAL
 */

if (builder.Environment.IsDevelopment())
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console() // Sink es el destino de donde se guardan los mensajes.
        .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("MusicStoreDB"),
            new MSSqlServerSinkOptions
            {
                AutoCreateSqlTable = true,
                TableName = "ApiLogs"
            }, restrictedToMinimumLevel: LogEventLevel.Warning)
        .CreateLogger();

    logger.Information("Hola soy Serilog");

    builder.Host.ConfigureLogging(options =>
    {
        //options.ClearProviders();
        options.AddSerilog(logger);
    });
}

builder.Services.AddCors(setup =>
{
    setup.AddPolicy(corsConfiguration, x =>
    {
        //x.WithOrigins("localhost", "musicstore.azurewebsites.net") para que las apis que creamos solo corran en musicstore.azurewebsites.net
        x.AllowAnyOrigin()//para que cualquier dominio pueda consumir nuestra api
            .AllowAnyHeader()//
            .AllowAnyMethod();
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddAutoMapper(options => options.AddProfile<AutoMapperProfiles>());

if (builder.Environment.IsDevelopment())
    builder.Services.AddTransient<IFileUploader, FileUploader>();
else
    builder.Services.AddTransient<IFileUploader, AzureBlobStorageUploader>();

builder.Services.AddDependencies();
// Add services to the container.
builder.Services.AddDbContext<MusicStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MusicStoreDB"));
    //Mostrar el detalle del EF Core.
    // options.LogTo(Console.WriteLine, LogLevel.Trace);

    // esto es para que en la consola muestre el parametro que esta enviando, solo debe estar habilitado en modo de desarrollo.
    options.EnableSensitiveDataLogging();

    //Utiliza el AsNoTracking por default en todos los querys de Seleccion
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddIdentity<MusicStoreUserIdentity, IdentityRole>(setup =>
{
    setup.Password.RequireNonAlphanumeric = false;
    setup.Password.RequiredUniqueChars = 0; //para que se pueda reperit las letras si es que esta en 0
    setup.Password.RequireUppercase = false;
    setup.Password.RequireLowercase = false;
    setup.Password.RequireDigit = false;
    setup.Password.RequiredLength = 8;

    setup.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<MusicStoreDbContext>()
    .AddDefaultTokenProviders(); // para que encripte las claves con la configuracion por default

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck("MusicStoreAPI", _ => HealthCheckResult.Healthy(), new[] { "servicio" })
    .AddTypeActivatedCheck<DiskHealthCheck>("Almacenamiento", HealthStatus.Healthy, new[] { "servicio" }, builder.Configuration)
    .AddTypeActivatedCheck<PingHealthCheck>("Google", HealthStatus.Healthy, new[] { "internet" }, "google.com")
    .AddTypeActivatedCheck<PingHealthCheck>("Azure", HealthStatus.Healthy, new[] { "internet" }, "azure.com")
    .AddTypeActivatedCheck<PingHealthCheck>("Host desconocido", HealthStatus.Healthy, new[] { "internet" }, "mocosoft.com.pe")
    .AddDbContextCheck<MusicStoreDbContext>("EF Core", null, new[] { "basedatos" });

//CONFIGURACION PARA LA AUTORIZACION Y AUTENTICACIÓN DE USUARIO
var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SigningKey"));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
//////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//SOLO DEBE IR EN ENTORNO DE DESARROLLO
    app.UseSwaggerUI();//SOLO DEBE IR EN ENTORNO DE DESARROLLO
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// HABILITAMOS EL CORS (EL FRONT-END TE LO AGRADECERA)
app.UseCors(corsConfiguration);

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("servicio")
    });

    endpoints.MapHealthChecks("/health/db", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("basedatos")
    });

    endpoints.MapHealthChecks("/health/externos", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("internet")
    });


});

app.MapControllers();

app.Run();
