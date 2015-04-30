using System.Collections;
using System.Collections.ObjectModel;

namespace CoAP_Analyzer_Client.Models
{
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
