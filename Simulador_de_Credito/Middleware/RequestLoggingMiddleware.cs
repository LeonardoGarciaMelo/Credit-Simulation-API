using Serilog;
using System.Diagnostics;

namespace Simulador_de_Credito.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly Serilog.ILogger TelemetriaLogger = Log.ForContext("TipoLog", "Telemetria");
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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
