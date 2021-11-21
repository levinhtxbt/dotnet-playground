using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog.Context;

namespace SS.Serilog
{
    /// <summary>
    ///    SerilogMiddleware is using to add new data for LogEntry
    /// </summary>
    public class SerilogMiddleware : IMiddleware
    {
        private SerilogMiddlewareOptions _options;

        public SerilogMiddleware(IOptions<SerilogMiddlewareOptions> options)
        {
            _options = options?.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(_options != null)
            {
                if (_options.GenerateRCID)
                {
                    context.Request.Headers["request-correlation-id"] = Guid.NewGuid().ToString();
                }
            }

            var requestId = context.Request.Headers["request-correlation-id"].ToString();
            using(LogContext.PushProperty("RCID", requestId))
            {
                await next.Invoke(context);
            }
        }
    }
}
