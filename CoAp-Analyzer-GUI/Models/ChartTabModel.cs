using CoAP_Analyzer_Client.Models;
using System.Collections;

namespace CoAP_Analyzer_GUI.Models
{
    public class ChartTabModel : BaseModel, IEnumerable
    {
        #region Members 
        ChartCreateModel _clm = new ChartCreateModel();
        int _index = 0;
        #endregion

        #region Construction
        public ChartTabModel()
        {
            CreateModel.Name = "Create Chart";
            CreateModel.Command = new RelayCommand(param => SharedData._command(_clm), param => true);
            Navigation = _clm.Navigation;
            Navigation.Add(_clm);
        }
        #endregion

        #region Properties
        public ChartCreateModel CreateModel { get { return _clm; } set { _clm = value; RaisePropertyChanged("CreateModel"); } }
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
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
            return new ListEnum<BaseModel>(Navigation);
        }
        #endregion

        
    }
}
