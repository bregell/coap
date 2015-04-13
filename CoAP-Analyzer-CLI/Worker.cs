#define DEBUG
using System;
using System.Threading;
using System.Collections.Generic;

namespace CoAP_Analyzer_Client
{
    public static class RND
    {
        static public Random r = new Random();
    }

    public class Worker
    {
        private bool _shouldStop;
        private bool _shouldPause;
        private int _parameter;
        public int _startTime { private get; set; }
        public Host _host { get; private set; }
        public int _rate { get; private set; }   
        public bool _done { get; private set; }
        public Func<int, Measure> _methodToRun { get; private set; }
        public List<Measure> _measure { get; private set; }

        public Worker(Host host, Func<int, Measure> f, int r, int param)
        {
            _host = host;
            _done = false;
            _shouldStop = false;
            _shouldPause = true;
            _rate = r;
            _parameter = param;
            _methodToRun = f;
            _measure = new List<Measure>();
            _startTime = 0;
        }

        public void Work()
        {
            waitCheck(1000, _startTime);
            while (!_shouldStop)
            {
                Measure m = _methodToRun(_parameter);
                _measure.Add(m);
                #if DEBUG
                System.Console.Write(_host.IP.ToString());
                System.Console.Write("@");
                System.Console.Write(_methodToRun.Method.Name);
                System.Console.Write(":");
                System.Console.Write(m.value);
                System.Console.Write(" ");
                System.Console.Write(m.unit);
                System.Console.Write("\n");
                #endif
                waitCheck(1000, _rate);
            }
            _done = true;
        }

        public void Run()
        {
            _shouldPause = false;
        }

        public void Pause()
        {
            _shouldPause = true;
        }

        public void Stop()
        {
            _shouldStop = true;
        }

        private void waitCheck(int _inc, int _time)
        {
            int _sleept = 0;
            while ((!_shouldStop && _sleept < _time) || _shouldPause)
            {
                Thread.Sleep(_inc);
                if (!_shouldPause)
                {
                    _sleept += _inc;
                }
            }
        }
    }

    public class WorkerComparer : IComparer<Worker>
    {
        public int Compare(Worker _w1, Worker _w2){
            return String.Compare(_w1._methodToRun.Method.Name, _w2._methodToRun.Method.Name);
        }

    }
}
