using LoadBalancer.Setting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoadBalancer.Controllers
{
    [Route("policy")]
    public class PolicyController : Controller
    {

        private readonly ILogger<PolicyController> _logger;
        protected BalancerSetting _balancerSetting { get; set; }


        public PolicyController(ILogger<PolicyController> logger, IOptions<BalancerSetting > settings = null)
        {
            _logger = logger;
            _balancerSetting = settings.Value;
        }

        [HttpGet]
        public IActionResult CheckBalancerPolicy()
        {
            var policy = _balancerSetting.Policy;
            _logger.LogInformation($"current policy is :{policy}");
            return Content($"current policy is :{policy}");
        }

        [HttpPut]
        public IActionResult UpdateBalancerPolicy(string mode)
        {
            _balancerSetting.Policy = mode;

            return Content($"update to policy: {mode}");
        }

    }
}