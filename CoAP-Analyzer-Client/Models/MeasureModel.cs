using System;
using System.Net;

namespace CoAP_Analyzer_Client.Models
{
    public class MeasureModel : BaseModel
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
        public MeasureModel(Measure _m, IPAddress _ip, string _name)
        {
            Measure = _m;
            IP = _ip;
            Name = _name;
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
                return _measure.Value;
            }
            set
            {
                _measure.Value = value;
                RaisePropertyChanged("Value");
            }
        }

        public string Unit
        {
            get
            {
                return _measure.Unit;
            }
            set
            {
                _measure.Unit = value;
                RaisePropertyChanged("Unit");
            }
        }

        public DateTime Time
        {
            get
            {
                return _measure.Time;
            }
            set
            {
                _measure.Time = value;
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
    }
}
