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

// Configuração inicial leve apenas para garantir que erros de startup sejam gravados.
// Se a aplicação quebrar este logger vai salvar o erro no arquivo.
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
    Log.Information("Iniciando a Aplicação...");

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
    Log.Information("Variáveis de ambiente carregadas.");

    //Serviços
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
            Title = "API Simulador de Crédito",
            Description = "API para o desafio técnico de simulação de crédito, desenvolvida em .NET 9."
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    //Conexão Oracle
    Log.Information("Configurando conexão com Oracle...");
    var oracleConnectionString = $"User Id={Environment.GetEnvironmentVariable("ORACLE_USER")};" +
                                $"Password={Environment.GetEnvironmentVariable("ORACLE_PASSWORD")};" +
                                $"Data Source={Environment.GetEnvironmentVariable("ORACLE_HOST")}:" +
                                $"{Environment.GetEnvironmentVariable("ORACLE_PORT")}/{Environment.GetEnvironmentVariable("ORACLE_SID")}";

    builder.Services.AddDbContext<OracleDbContext>(options =>
        options.UseOracle(oracleConnectionString));

    //Conexão SQLite
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

    //Verifica se os bancos estão disponíveis
    builder.Services.AddHealthChecks()
    .AddDbContextCheck<OracleDbContext>(
        name: "OracleDB",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)
    .AddDbContextCheck<SqliteDbContext>(
        name: "SQLiteDB",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);


    var app = builder.Build();
    Log.Information("Build da aplicação realizado com sucesso.");

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = HealthCheckResponseWriter.WriteResponse
    });
    app.UseMiddleware<RequestLoggingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        });
        Log.Information("Swagger Habilitado.");
    }

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
            Log.Fatal(ex, "Erro crítico ao criar o banco de dados SQLite.");
            throw;
        }
    }

    Log.Information("Aplicação iniciada e aguardando requisições.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha no startup da aplicação");
}
finally
{
    Log.CloseAndFlush();
}