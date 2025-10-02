using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Simulador_de_Credito.Data;
using Simulador_de_Credito.Service;

var builder = WebApplication.CreateBuilder(args);
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
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        /*options.SwaggerEndpoint("/swagger/v1/swagger.json", "Simulador_de_crédito v1");
       
        options.RoutePrefix = string.Empty; */
        app.UseSwagger();
        app.UseSwaggerUI();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

// Creating SQLite Database and Simulacoes table automatically
using (var scope = app.Services.CreateScope())
{
    var sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    sqliteContext.Database.EnsureCreated();
}

app.Run();
