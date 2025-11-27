using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Simulador_de_Credito.Utils
{
    public static class HealthCheckResponseWriter
    {
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
