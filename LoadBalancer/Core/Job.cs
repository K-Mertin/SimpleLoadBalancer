using System;

namespace LoadBalancer.Core
{
    public class Job
    {
        private int _processTime;
        private string _uri;

        public Job(string uri)
        {            
            Random random = new Random();
            _processTime= random.Next(1000,10000);
            _uri = uri;
        }

        public int getProcessTime()
        {
            return _processTime;
        }
        public int getUri()
        {
            return _processTime;
        }
    }
}
