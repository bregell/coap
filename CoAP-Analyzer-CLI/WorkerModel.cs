using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CoAP_Analyzer_Client
{
    public class MeasureModel : INotifyPropertyChanged
    {
        #region Members
        Measure _measure;
        IPAddress _ip;
        #endregion

        #region Construction
        public MeasureModel()
        {
            _measure = null;
            _ip = null;
        }

        public MeasureModel(Measure _m, IPAddress _ip)
        {
            Measure = _m;
            IP = _ip;
        }
        #endregion

        #region Properties
        public Measure Measure
        {
            get
            {
                return _measure;
            }
            set
            {
                _measure = value;
            }
        }

        public double Value
        {
            get
            {
                return _measure.value;
            }
            set
            {
                _measure.value = value;
                RaisePropertyChanged("Value");
            }
        }

        public string Unit
        {
            get
            {
                return _measure.unit;
            }
            set
            {
                _measure.unit = value;
                RaisePropertyChanged("Unit");
            }
        }

        public DateTime Time
        {
            get
            {
                return _measure.time;
            }
            set
            {
                _measure.time = value;
                RaisePropertyChanged("Time");
            }
        }
        public IPAddress IP
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                RaisePropertyChanged("IP");
            }
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
                return _worker._methodToRun.Method.Name.ToString();
            }
        }

        public IPAddress IP
        {
            get
            {
                return _worker._host.IP;
            }
        }

        public int Rate
        {
            get
            {
                return _worker._rate;
            }
        }

        public ObservableCollection<MeasureModel> Measure
        {
            get
            {
                return _worker._measures;
            }
        }
        #endregion

        #region Construction
        public WorkerModel()
        {
            _worker = null;
        }
        public WorkerModel(Worker _w)
        {
            _worker = _w;
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
