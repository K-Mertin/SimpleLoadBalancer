using LoadBalancer.Core;
using LoadBalancer.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LoadBalancer.MiddleWare
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly WorkerSetting _workerSetting;

        public ProxyMiddleware(RequestDelegate next, ILogger<BalancerMiddleware> logger, IOptions<WorkerSetting> settings = null)
        {
            _next = next;
            _logger = logger;
            _workerSetting = settings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var fileUri = context.Request.Query["uri"].ToString();               
                var worker = context.Items["worker"] as Worker;

                
                //TODO send a real request to file server

                //fake file service
                if (_workerSetting.Mode.Equals("Queue"))
                {
                    if (worker.PutFileInQueue(fileUri))
                        await context.Response.WriteAsync("Job Added");
                    else
                        await context.Response.WriteAsync("Failed");
                }
                else
                {
                    if(worker.ConvertFileDirectly(fileUri))
                        await context.Response.WriteAsync("Success");
                    else
                        await context.Response.WriteAsync("Failed");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in ProxyMiddleware {e.Message}\n");
            }
        }

    }
    public static class ProxyMiddlewareExtensions
    {
        public static IApplicationBuilder UseProxyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProxyMiddleware>();
        }
    }

}
