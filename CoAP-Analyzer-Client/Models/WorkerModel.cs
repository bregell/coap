using System;
using System.Collections.ObjectModel;
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

        public string Path
        {
            get
            {
                return Worker.Resource.Path;
            }
        }

        public string MethodName
        {
            get
            {
                return Worker.Resource.Name;
            }
            set
            {
                Worker.Resource.Name = value;
                RaisePropertyChanged("MethodName");
            }
        }

        public IPAddress IP
        {
            get
            {
                return Worker.Host.IP;
            }
            set
            {
                Worker.Host.IP = value;
                RaisePropertyChanged("IP");
            }
        }

        public int Rate
        {
            get
            {
                return Worker.Rate;
            }
            set
            {
                Worker.Rate = value;
                RaisePropertyChanged("Rate");
            }
        }

        public int Timeout
        {
            get
            {
                return Worker.Resource.Timeout;
            }
            set
            {
                Worker.Resource.Timeout = value;
                RaisePropertyChanged("Timeout");
            }
        }

        public ObservableCollection<MeasureModel> Measure
        {
            get
            {
                return Worker.Measures;
            }
        }
        #endregion

        #region Construction
        public WorkerModel(Host host, Func<Resource, Measure> f, Resource param)
        {
            Worker = new Worker(host, f, param);
        }
        #endregion

        #region Methods

        #endregion
    }
}
