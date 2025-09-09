using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Simulador_de_cr�dito.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // A linha abaixo define um "documento" de API.
    // O primeiro par�metro "v1" � um identificador �nico para a vers�o.
    // O segundo par�metro � o objeto OpenApiInfo que cont�m os metadados da sua API.
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        // O campo 'Version' � crucial e resolve o seu erro.
        Version = "v1",
        Title = "API Simulador de Cr�dito",
        Description = "API para o desafio t�cnico de simula��o de cr�dito, desenvolvida em .NET 9."
    });
});

// Configurar Oracle DbContext para a tabela Produto
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Configurar SQLite DbContext para a tabela Simulations
builder.Services.AddDbContext<SqliteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // A linha abaixo aponta a UI para o endpoint do JSON da sua defini��o "v1".
        // O texto "Simulador_de_cr�dito v1" � o que aparece no dropdown da sua imagem.
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Simulador_de_cr�dito v1");
        
        // (Opcional) Redireciona a raiz do site para o Swagger UI
        options.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Creating SQLite Database and Simulacoes table automatically
using (var scope = app.Services.CreateScope())
{
    var sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    sqliteContext.Database.EnsureCreated();
}

app.Run();
