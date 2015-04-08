using System;
using System.Threading;

namespace CoAP_Analyzer_CLI
{
    public class Worker
    {
        private volatile bool _shouldStop = false;
        public Host h;
        public int rate;
        public volatile bool _done = false;
        public Action<int> methodToRun;

        public void Work()
        {
            while (!_shouldStop)
            {
                long now = DateTime.Now.ToBinary();
                methodToRun(rate);
                if (DateTime.Now.ToBinary() < (now + rate))
                {
                    Thread.Sleep(System.TimeSpan.FromMilliseconds(DateTime.Now.ToBinary() - (now + rate)));
                }
                
            }
            _done = true;
            methodToRun.Method.Name.
        }
        public void Stop()
        {
            _shouldStop = true;
        }
    }
}
