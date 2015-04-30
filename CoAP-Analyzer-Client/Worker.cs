#define DEBUG
using CoAP_Analyzer_Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace CoAP_Analyzer_Client
{
    public static class RND
    {
        static public Random r = new Random();
    }

    public class Worker
    {
        #region Members
        bool _shouldStop = false;
        bool _shouldPause = true;
        bool _done = false; 
        int _startTime = 0;

        Host _host;
        Resource _resource;
        Func<Resource, Measure> _methodToRun;
        ObservableCollection<MeasureModel> _measures = new ObservableCollection<MeasureModel>();
        #endregion

        #region Construction
        public Worker(Host _host, Func<Resource, Measure> _func, Resource _res)
        {
            Host = _host;
            Resource = _res;
            MethodToRun = _func;
        }
        #endregion

        #region Properties
        public int Rate
        {
            get
            {
                return Resource.Rate;
            }
            set
            {
                Resource.Rate = value;
            }
        }
        public Resource Resource
        {
            get
            {
                return _resource;
            }
            set
            {
                _resource = value;
            }

        }
        public Func<Resource, Measure> MethodToRun
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
        public int StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }

        public bool Running
        {
            get
            {
                return !_shouldPause;
            }
        }

        public bool Done
        {
            get
            {
                return _done;
            }
        }
        #endregion

        #region Methods
        public void Work()
        {
            waitCheck(1000, _startTime);
            while (!_shouldStop)
            {
                Measure m = MethodToRun(Resource);
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    _measures.Add(new MeasureModel(m, Host.IP, MethodToRun.Method.Name));
                });               
                #if DEBUG
                System.Console.Write(Host.IP.ToString());
                System.Console.Write("@");
                System.Console.Write(MethodToRun.Method.Name);
                System.Console.Write(":");
                System.Console.Write(m.Value);
                System.Console.Write(" ");
                System.Console.Write(m.Unit);
                System.Console.Write("\n");
                #endif
                waitCheck(1000, Resource.Rate);
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
