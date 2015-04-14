using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace CoAP_Analyzer_Client.Models
{
    public class WorkerModel : INotifyPropertyChanged
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
            }
        }

        public string Name
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
        }

        public int Rate
        {
            get
            {
                return _worker.Rate;
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
            _worker = new Worker(host, f, r, param);
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class WorkerListModel
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
            }
        }
        #endregion
    }
}
