using LoadBalancer.Core;
using System.Linq;

namespace LoadBalancer.Setting
{
    public class BalancerPolicy
    {
        private readonly string _policy;
        private readonly Balancer _balancer;

        public BalancerPolicy(Balancer balancer, string policy)
        {
            _balancer = balancer;
            _policy = policy;
        }
        public Worker GetWorker()
        {
            if (_policy == "RoundRobin")
            {
                return RoundRobin();
            }
            else if (_policy == "RequestSend")
            {
                return  RequestSend();
            }
            else if (_policy == "WorkLoad")
            {
                return WorkLoad();
            }           
            return RoundRobin();
        }
        private Worker RoundRobin()
        {
            Worker worker = _balancer.workers[_balancer.nextWorkerIndex];
            _balancer.workerLoad[_balancer.nextWorkerIndex] += 1;
            _balancer.nextWorkerIndex = (_balancer.nextWorkerIndex % _balancer.workers.Count()) + 1;
            return worker;
        }
        private Worker RequestSend()
        {
            var key = _balancer.workerLoad.OrderBy(x => x.Value).FirstOrDefault().Key;

            if (_balancer.workerLoad[key] + 1 < long.MaxValue)
            {
                _balancer.workerLoad[key] += 1;
            }
            else
            {
                for (int i = 1; i <= _balancer.workerLoad.Count; i++)
                {
                    _balancer.workerLoad[i] = 0;
                }
            }
            return _balancer.workers[key];
        }
        private Worker WorkLoad()
        {
            var key = _balancer.workers.OrderBy(x => x.Value.JobsInQueue()).FirstOrDefault().Key;
            return _balancer.workers[key];
        }

    }
}
