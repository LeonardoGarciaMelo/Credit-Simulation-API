using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Simulador_de_Credito.Data;
using Simulador_de_Credito.Middleware;
using Simulador_de_Credito.Service;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    // Ignora logs do sistema da Microsoft
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)

    // Saída 1: Console (Para você ver rodando)
    .WriteTo.Console()

    // Saída 2: Arquivo .txt
    // Cria na pasta /logs do projeto. Um arquivo novo por dia.
    .WriteTo.File("logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")

    // Saída 3: Seq (Docker)
    // Só funciona se o Docker estiver rodando, mas não quebra se não estiver.
    .WriteTo.Seq("http://localhost:5341")

    .CreateLogger();

// Substitui o logger padrão do .NET pelo Serilog
builder.Host.UseSerilog();

//servicos
builder.Services.AddScoped<CalculoService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<SimulacaoService>();

DotEnv.Load();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

var oracleConnectionString = $"User Id={Environment.GetEnvironmentVariable("ORACLE_USER")};" +
                            $"Password={Environment.GetEnvironmentVariable("ORACLE_PASSWORD")};" +
                            $"Data Source={Environment.GetEnvironmentVariable("ORACLE_HOST")}:" +
                            $"{Environment.GetEnvironmentVariable("ORACLE_PORT")}/{Environment.GetEnvironmentVariable("ORACLE_SID")}";
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseOracle(oracleConnectionString));

builder.Services.AddDbContext<SqliteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowReactApp");

app.UseAuthorization();

// Creating SQLite Database and Simulacoes table automatically
using (var scope = app.Services.CreateScope())
{
    var sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    sqliteContext.Database.EnsureCreated();
}

app.Run();
