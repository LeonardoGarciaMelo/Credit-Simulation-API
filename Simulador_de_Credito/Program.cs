using dotenv.net;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Simulador_de_Credito.Data;
using Simulador_de_Credito.Middleware;
using Simulador_de_Credito.Service;
using Simulador_de_Credito.Utils;
using System.Reflection;

// Configura��o inicial leve apenas para garantir que erros de startup sejam gravados.
// Se a aplica��o quebrar este logger vai salvar o erro no arquivo.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando a Aplica��o...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)

        //Console
        .WriteTo.Console()

        //Arquivo de TELEMETRIA
        .WriteTo.Logger(l => l
            .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("TipoLog") &&
                                         e.Properties["TipoLog"].ToString().Contains("Telemetria"))
            .WriteTo.File("logs/log-telemetria-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}"))

        //Arquivo GERAL
        .WriteTo.Logger(l => l
            .Filter.ByExcluding(e => e.Properties.ContainsKey("TipoLog") &&
                                     e.Properties["TipoLog"].ToString().Contains("Telemetria"))
            .WriteTo.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))

        //Docker
        .WriteTo.Seq("http://localhost:5341"));

    DotEnv.Load();
    Log.Information("Vari�veis de ambiente carregadas.");

    //Servi�os
    builder.Services.AddScoped<CalculoService>();
    builder.Services.AddScoped<ProdutoService>();
    builder.Services.AddScoped<SimulacaoService>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    //Swagger
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "API Simulador de Cr�dito",
            Description = "API para o desafio t�cnico de simula��o de cr�dito, desenvolvida em .NET 8."
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    //Conex�o Oracle
    Log.Information("Configurando conex�o com Oracle...");
    var oracleConnectionString = $"User Id={Environment.GetEnvironmentVariable("ORACLE_USER")};" +
                                $"Password={Environment.GetEnvironmentVariable("ORACLE_PASSWORD")};" +
                                $"Data Source={Environment.GetEnvironmentVariable("ORACLE_HOST")}:" +
                                $"{Environment.GetEnvironmentVariable("ORACLE_PORT")}/{Environment.GetEnvironmentVariable("ORACLE_SID")}";

    builder.Services.AddDbContext<OracleDbContext>(options =>
        options.UseOracle(oracleConnectionString));

    //Conex�o SQLite
    builder.Services.AddDbContext<SqliteDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

    // CORS
    builder.Services.AddCors(options => {
        options.AddPolicy("AllowReactApp",
            builder =>
            {
                builder.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    //Verifica se os bancos est�o dispon�veis
    builder.Services.AddHealthChecks()
    .AddDbContextCheck<OracleDbContext>(
        name: "OracleDB",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)
    .AddDbContextCheck<SqliteDbContext>(
        name: "SQLiteDB",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);


    var app = builder.Build();
    Log.Information("Build da aplica��o realizado com sucesso.");

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = HealthCheckResponseWriter.WriteResponse
    });
    app.UseMiddleware<RequestLoggingMiddleware>();

    //if (app.Environment.IsDevelopment())
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    });
    Log.Information("Swagger Habilitado.");
    

    app.UseHttpsRedirection();

    app.UseCors("AllowReactApp");

    app.UseAuthorization(); 

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            Log.Information("Verificando banco de dados SQLite...");
            var sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
            sqliteContext.Database.EnsureCreated();
            Log.Information("Banco de dados verificado/criado.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Erro cr�tico ao criar o banco de dados SQLite.");
            throw;
        }
    }

    Log.Information("Aplica��o iniciada e aguardando requisi��es.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha no startup da aplica��o");
}
finally
{
    Log.CloseAndFlush();
}