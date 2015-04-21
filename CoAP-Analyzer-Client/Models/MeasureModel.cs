using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using CoAP_Analyzer_Client;

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
    }

    public class MeasureListModel : BaseModel, IEnumerable
    {
        #region Members
        ObservableCollection<MeasureModel> _measures;
        #endregion

        #region Construction
        public MeasureListModel()
        {
            Measures = new ObservableCollection<MeasureModel>();
        }
        #endregion

        #region Properties
        public ObservableCollection<MeasureModel> Measures
        {
            get
            {
                return _measures;
            }
            set
            {
                _measures = value;
                RaisePropertyChanged("Measures");
            }
        }

        public string Unit
        {
            get
            {
                return Measures.Count != 0 ? Measures[0].Unit : "Unit";
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
            return new ListEnum<MeasureModel>(_measures);
        }
        #endregion 
    }
}
