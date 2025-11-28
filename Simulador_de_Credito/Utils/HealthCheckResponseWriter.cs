using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Simulador_de_Credito.Utils
{
    /// <summary>
    /// Classe utilitária responsável por personalizar a formatação da resposta do endpoint de Health Check.
    /// </summary>
    /// <remarks>
    /// Por padrão, o middleware de Health Check do .NET retorna apenas uma string simples (ex: "Healthy").
    /// Esta classe intercepta o relatório de saúde (<see cref="HealthReport"/>) e o transforma em um objeto JSON detalhado,
    /// expondo o status individual de cada dependência (Oracle, SQLite), latência e eventuais erros.
    /// </remarks>
    public static class HealthCheckResponseWriter
    {
        /// <summary>
        /// Serializa o relatório de saúde e escreve no corpo da resposta HTTP.
        /// </summary>
        /// <remarks>
        /// A resposta gerada inclui:
        /// <list type="bullet">
        /// <item>Status geral da aplicação.</item>
        /// <item>Duração total da verificação.</item>
        /// <item>Lista detalhada de cada verificação configurada (Status, Descrição, Duração e Exceção).</item>
        /// </list>
        /// </remarks>
        /// <param name="context">O contexto HTTP da requisição atual, usado para definir o Content-Type e escrever a resposta.</param>
        /// <param name="report">O relatório gerado pelo serviço de Health Check contendo os resultados das verificações.</param>
        /// <returns>Uma <see cref="Task"/> que representa a operação assíncrona de escrita na resposta.</returns>
        public static Task WriteResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration,
                timestamp = DateTime.UtcNow,
                results = report.Entries.Select(e => new
                {
                    key = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration,
                    exception = e.Value.Exception?.Message
                })
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            return context.Response.WriteAsync(json);
        }
    }
}
