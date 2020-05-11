using LoadBalancer.Setting;
using System.Collections.Generic;

namespace LoadBalancer.Core
{
    public class Balancer
    {
        public int nextWorkerIndex;
        public Dictionary<int, Worker> workers = new Dictionary<int, Worker>();
        public Dictionary<int, long> workerLoad = new Dictionary<int, long>();
        private BalancerSetting _balancerSetting;
        public Balancer(BalancerSetting balancerSetting)
        {
            _balancerSetting = balancerSetting;
            Init();
        }

        internal void Init()
        {
            int workerNumbers = _balancerSetting.WorkerNumbers;
            for (int i = 1; i <= workerNumbers; i++)
            {                
                workers.Add(i, new Worker(i));
                workerLoad.Add(i, 0);
            }
            nextWorkerIndex = 1;

        }
    }
}
