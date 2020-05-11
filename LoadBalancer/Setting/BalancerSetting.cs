using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancer.Setting
{
    public class BalancerSetting
    {
        public string Policy { get; set; }
        public int WorkerNumbers { get; set; }
    }
}
