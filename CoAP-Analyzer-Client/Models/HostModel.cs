using System.Collections.ObjectModel;
using System.Net;

namespace CoAP_Analyzer_Client.Models
{
    public class HostModel : BaseModel
    {
        #region Members
        Host _host = new Host { IP = null };
        WorkerListModel _workers = new WorkerListModel();
        int _rate;
        #endregion

        #region Construction
        public HostModel()
        {
            Name = "Host";
        }

        public HostModel(IPAddress _ip)
        {
            _host = new Host { IP = _ip };
        }
        #endregion

        #region Properties
        public Host Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
                RaisePropertyChanged("Host");
            }
        }

        public WorkerListModel Workers
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

        public IPAddress IP
        {
            get
            {
                return _host.IP;
            }
            set
            {
                _host.IP = value;
                RaisePropertyChanged("IP");
            }
        }
        public int Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                RaisePropertyChanged("Rate");
            }
        }
        #endregion
    }

    public class HostListModel : BaseModel
    {
        #region Members
        ObservableCollection<HostModel> _hosts;
        #endregion

        #region Construction
        public HostListModel()
        {
            _hosts = new ObservableCollection<HostModel>();
        }
        #endregion

        #region Properties
        public ObservableCollection<HostModel> Hosts
        {
            get
            {
                return _hosts;
            }
            set
            {
                _hosts = value;
            }
        }
        #endregion
    }
}
