using CoAP_Analyzer_Client;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace CoAP_Analyzer_Client.Models
{
    public class WorkerModel : BaseModel
    {
        #region Members
        Worker _worker;
        #endregion

        #region Properties
        public Worker Worker
        {
            get
            {
                return _worker;
            }
            set
            {
                _worker = value;
                RaisePropertyChanged("Worker");
            }
        }

        public string MethodName
        {
            get
            {
                return _worker.MethodToRun.Method.Name.ToString();
            }
        }

        public IPAddress IP
        {
            get
            {
                return _worker.Host.IP;
            }
            set
            {
                _worker.Host.IP = value;
                RaisePropertyChanged("IP");
            }
        }

        public int Rate
        {
            get
            {
                return _worker.Rate;
            }
            set
            {
                _worker.Rate = value;
                RaisePropertyChanged("Rate");
            }
        }

        public ObservableCollection<MeasureModel> Measure
        {
            get
            {
                return _worker.Measures;
            }
        }
        #endregion

        #region Construction
        public WorkerModel(Host host, Func<int, Measure> f, int r, int param)
        {
            Worker = new Worker(host, f, r, param);
        }
        #endregion
    }

    public class WorkerListModel : BaseModel, IEnumerable
    {
        #region Members
        ObservableCollection<WorkerModel> _workers;
        #endregion

        #region Construction
        public WorkerListModel()
        {
            _workers = new ObservableCollection<WorkerModel>();
        }
        #endregion

        #region Properties
        public ObservableCollection<WorkerModel> Workers
        {
            get
            {
                return _workers;
            }
            set
            {
                _workers = value;
                RaisePropertyChanged("Workers");
            }
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return new ListEnum<WorkerModel>(_workers);
        }
        #endregion 
    }
}
