using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CoAP_Analyzer_Client;

namespace CoAP_Analyzer_Client.Models
{
    public class HostModel : BaseModel
    {
        #region members
        Host _host;
        int _rate;
        #endregion

        #region Construction
        public HostModel()
        {
            _host = new Host { IP = null };
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
