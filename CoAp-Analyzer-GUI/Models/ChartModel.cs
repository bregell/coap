using CoAP_Analyzer_Client.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Windows;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoAP_Analyzer_GUI.Models
{
    public class ChartModel : BaseModel
    {
        #region Members
        private PlotModel _plotModel;
        private List<LineSeries> _lineSeries;
        #endregion

        #region Construction
        public ChartModel()
        {
            Model = new PlotModel();
            Model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Unit = "Time" });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Unit", });
            Model.Title = "Test";
            LineSeries =  new List<LineSeries>();
            LineSeries.Add(new LineSeries());
            foreach (LineSeries _ls in LineSeries)
            {
                Model.Series.Add(_ls);
            }
            
        }
        #endregion

        #region Properties
        public List<LineSeries> LineSeries
        {
            get
            {
                return _lineSeries;
            }
            set
            {
                _lineSeries = value;
                RaisePropertyChanged("LineSerie");
            }
        }

        public PlotModel Model
        {
            get { return _plotModel; }
            set { _plotModel = value; RaisePropertyChanged("Model"); }
        }
        #endregion

        #region Methods
        public void ClearModel()
        {
            LineSeries[0].Points.Clear();
            RaisePropertyChanged("LineSerie");
        }

        public void UpdateModel(LineSeries _ls, ObservableCollection<MeasureModel> _measures)
        {
            foreach (MeasureModel _m in _measures)
            {
                _ls.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_m.Time), _m.Value));
            }
            RaisePropertyChanged("LineSerie");
        }
        #endregion  
    }
}
