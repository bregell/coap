using System;
using System.Threading;
using System.Collections.Generic;

namespace CoAP_Analyzer_CLI
{
    public static class RND
    {
        static public Random r = new Random();
    }
    class Worker
    {
        
        private bool _shouldStop;
        public Host h { get; private set; }
        public int rate { get; private set; }
        private int parameter { get; set; }
        public bool _done { get; private set; }
        public Func<int, Measure> methodToRun { get; private set; }
        public List<Measure> measure { get; private set; }

        public Worker(Host host, Func<int, Measure> f, int r, int param)
        {
            h = host;
            _done = false;
            _shouldStop = false;
            rate = r;
            parameter = param;
            methodToRun = f;
            measure = new List<Measure>();
        }

        public void Work()
        {
           
            Thread.Sleep((int)(RND.r.NextDouble() * rate));
            while (!_shouldStop)
            {
                measure.Add(methodToRun(parameter));
                Thread.Sleep(rate);
            }
            _done = true;
        }
        public void Stop()
        {
            _shouldStop = true;
        }
    }
}
