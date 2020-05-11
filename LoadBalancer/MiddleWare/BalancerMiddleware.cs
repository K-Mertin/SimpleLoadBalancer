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
    public class BalancerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly BalancerSetting _balancerSetting;
        private readonly Balancer _balancer;


        public BalancerMiddleware(RequestDelegate next, ILogger<BalancerMiddleware> logger, IOptions<BalancerSetting> settings = null)
        {
            _next = next;
            _logger = logger;
            _balancerSetting = settings.Value;
            _balancer = new Balancer(_balancerSetting);
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                Worker worker = new BalancerPolicy(_balancer, _balancerSetting.Policy).GetWorker();
               
                context.Items["worker"] = worker;

                _logger.LogInformation($"Request {context.Request.Query["uri"].ToString()} sent to :{worker.getId}");

                await _next(context);
                
            }
            catch(Exception e)
            {
                _logger.LogError($"Error in BalancerMiddleware {e.Message}");
            }
        }
      
    }
    public static class BalancerMiddlewareExtensions
    {
        public static IApplicationBuilder UseBalancerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BalancerMiddleware>();
        }
    }

}
