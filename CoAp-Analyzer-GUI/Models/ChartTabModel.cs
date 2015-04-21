using CoAP_Analyzer_Client.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Windows;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoAP_Analyzer_GUI.Models
{
    public class ChartTabModel : BaseModel, IEnumerable
    {
        #region Members 
        ChartCreateModel _clm;
        #endregion

        #region Construction
        public ChartTabModel()
        {
            CreateModel = new ChartCreateModel();
            CreateModel.Name = "Create Chart";
            CreateModel.Command = new RelayCommand(param => SharedData._command(_clm), param => true);
            Navigation = _clm.Navigation;
            Navigation.Add(_clm);
        }
        #endregion

        #region Properties
        public ChartCreateModel CreateModel { get { return _clm; } set { _clm = value; RaisePropertyChanged("CreateModel"); } }
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
