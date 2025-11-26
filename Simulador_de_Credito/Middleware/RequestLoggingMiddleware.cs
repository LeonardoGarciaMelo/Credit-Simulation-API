using Serilog;
using System.Diagnostics;

namespace Simulador_de_Credito.Middleware
{
    /// <summary>
    /// Middleware de observabilidade responsável por interceptar todas as requisições HTTP,
    /// cronometrar o tempo de execução e registrar logs estruturados de auditoria.
    /// </summary>
    /// <remarks>
    /// Este componente atua como um "Cross-Cutting Concern" (Interesse Transversal), centralizando
    /// a lógica de log de tráfego para evitar repetição de código nos Controllers.
    /// </remarks>
    public class RequestLoggingMiddleware
    {
        /// <summary>
        /// Logger contextual específico para Telemetria.
        /// </summary>
        /// <remarks>
        /// Este logger é pré-configurado com a propriedade <c>{TipoLog: "Telemetria"}</c>.
        /// Isso permite que o Serilog (no Program.cs) filtre e direcione estes logs 
        /// para um arquivo separado (<c>log-telemetria.txt</c>), mantendo o log principal limpo.
        /// </remarks>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Inicializa uma nova instância do middleware.
        /// </summary>
        /// <param name="next">O próximo delegado no pipeline de requisição do ASP.NET Core.</param>
        private static readonly Serilog.ILogger TelemetriaLogger = Log.ForContext("TipoLog", "Telemetria");
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Executa a lógica de interceptação, cronometragem e log da requisição.
        /// </summary>
        /// <param name="context">O contexto HTTP atual.</param>
        /// <returns>Uma tarefa assíncrona que representa a execução do pipeline.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            string metodo = context.Request.Method;
            string path = context.Request.Path;

            try
            {
                //Deixa a requisição seguir para o Controller
                await _next(context);
            }
            catch (Exception ex) 
            {
                stopwatch.Stop();
                // O Serilog grava automaticamente o StackTrace completo no arquivo
                TelemetriaLogger.Error(ex, "FALHA | {Metodo} {Path} | Tempo: {TempoGasto}ms",
                    metodo, path, stopwatch.ElapsedMilliseconds);

                throw;
            }
            finally 
            {
                stopwatch.Stop();
                long tempoGasto = stopwatch.ElapsedMilliseconds;
                int statusCode = context.Response.StatusCode;

                bool sucesso = statusCode >= 200 && statusCode < 300;

                if (!path.Contains("swagger") && !path.Contains("seq"))
                {
                    if (!sucesso)
                    {
                        TelemetriaLogger.Warning("ALERTA | {Metodo} {Path} | Status: {StatusCode} | Tempo: {TempoGasto}ms",
                            metodo, path, statusCode, tempoGasto);
                    }
                    else
                    {
                        TelemetriaLogger.Information("SUCESSO | {Metodo} {Path} | Status: {StatusCode} | Tempo: {TempoGasto}ms",
                            metodo, path, statusCode, tempoGasto);
                    }
                }
            }
        }
    }
}
