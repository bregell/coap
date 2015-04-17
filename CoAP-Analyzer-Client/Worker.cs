#define DEBUG
using System;
using System.Windows;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_Client
{
    public static class RND
    {
        static public Random r = new Random();
    }

    public class Worker
    {
        #region Members
        private bool _shouldStop;
        private bool _shouldPause;
        private int _parameter;
        public int _startTime;
        private Host _host;
        private int _rate;
        public bool _done;
        private Func<int, Measure> _methodToRun;
        private ObservableCollection<MeasureModel> _measures;
        #endregion

        #region Construction
        public Worker(Host host, Func<int, Measure> f, int r, int param)
        {
            _host = host;
            _done = false;
            _shouldStop = false;
            _shouldPause = true;
            _rate = r;
            _parameter = param;
            _methodToRun = f;
            _measures = new ObservableCollection<MeasureModel>();
            _startTime = 0;
        }
        #endregion

        #region Properties
        public int Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
            }

        }
        public int Parameter
        {
            get
            {
                return _parameter;
            }
            set
            {
                _parameter = value;
            }

        }
        public Func<int, Measure> MethodToRun
        {
            get
            {
                return _methodToRun;
            }
            set
            {
                _methodToRun = value;
            }

        }
        public Host Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }
        public ObservableCollection<MeasureModel> Measures
        {
            get
            {
                return _measures;
            }
            set
            {
                _measures = value;
            }
        }
        #endregion

        #region Methods
        public void Work()
        {
            waitCheck(1000, _startTime);
            while (!_shouldStop)
            {
                Measure m = _methodToRun(_parameter);
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    _measures.Add(new MeasureModel(m, _host.IP));
                });               
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
        #endregion
    }

    public class WorkerComparer : IComparer<Worker>
    {
        public int Compare(Worker _w1, Worker _w2){
            return String.Compare(_w1.MethodToRun.Method.Name, _w2.MethodToRun.Method.Name);
        }

    }
}
