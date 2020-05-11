using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancer.Core
{
    public class Worker: IDisposable
    {
        private BlockingCollection<Job> _jobsCollection = new BlockingCollection<Job>();
        private int _id;

        public Worker(int id)
        {
            _id = id;
            Task t = Task.Run(() => BackgroundWorker());
        }

        public int getId => _id;
        
        public bool ConvertFileDirectly(string uri)
        {
            Job job = new Job(uri);

            Task.Delay(job.getProcessTime()).Wait();    

            if (new Random().Next(10) > 8)
                return false;

            return true;            
        }
        
        public bool PutFileInQueue(string uri)
        {
            Job job = new Job(uri);
            _jobsCollection.Add(job);

            return true;
        }

        public int JobsInQueue()
        {
            return _jobsCollection.Count();
        }

        private void BackgroundWorker()
        {
            foreach (var job in _jobsCollection.GetConsumingEnumerable())
            {
                Console.WriteLine($"Worker {_id} is processing {job.getUri()}");
                Task.Delay(job.getProcessTime()).Wait();

                if (new Random().Next(10) > 8)
                {
                    Console.WriteLine($"Worker {_id} failed {job.getUri()}");

                    _jobsCollection.Add(job);
                    continue;
                }                    
                Console.WriteLine($"Worker {_id} finish {job.getUri()}");
            }
        }

        public void Dispose()
        {
            _jobsCollection.CompleteAdding();
        }
    }
}
