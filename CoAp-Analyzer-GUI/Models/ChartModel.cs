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
    public class ChartModel : BaseModel
    {
        #region Members
        PlotModel _plotModel;
        ObservableCollection<Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>>> _series;
        #endregion

        #region Construction
        public ChartModel()
        {
            Model = new PlotModel();
            Model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Unit = "Time" });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Unit", });
            Model.Title = "New Plot";
            Series = new ObservableCollection<Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>>>();
        }
        #endregion

        #region Properties
        public  ObservableCollection<Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>>> Series
        {
            get { return _series; }
            set { _series = value; RaisePropertyChanged("Series"); }
        }

        public PlotModel Model
        {
            get { return _plotModel; }
            set { _plotModel = value; RaisePropertyChanged("Model"); }
        }
        #endregion

        #region Methods
        public void AddSeries(WorkerModel _wm)
        {
            LineSeries _ls = new LineSeries();
            _ls.Title = _wm.IP.ToString()+_wm.MethodName;
            foreach(MeasureModel _m in _wm.Measure){
                _ls.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_m.Time), _m.Value));
            }
            Model.Series.Add(_ls);
            _wm.Measure.CollectionChanged += Measures_CollectionChanged;
            Series.Add(new Tuple<
                LineSeries, 
                NotifyCollectionChangedEventHandler, 
                ObservableCollection<MeasureModel>>
                (_ls, Measures_CollectionChanged, _wm.Measure));
        }

        public void RemoveSeries(WorkerModel _wm)
        {
            Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>> _tp;
            _tp = Series.Single(x => x.Item3.Equals(_wm.Measure));
            _tp.Item3.CollectionChanged -= _tp.Item2;
            Model.Series.Remove(_tp.Item1);
            Series.Remove(_tp);
        }

        /**
         * 
         **/
        public void ClearSeries()
        {
            Model.Series.Clear();
            Series.Clear();
            RaisePropertyChanged("LineSeries");
        }

        //public void UpdateSeries(LineSeries _ls, ObservableCollection<MeasureModel> _measures)
        //{
        //    foreach (MeasureModel _m in _measures)
        //    {
        //        _ls.Points.Add(new DataPoint(DateTimeAxis.ToDouble(_m.Time), _m.Value));
        //    }
        //    RaisePropertyChanged("LineSeries");
        //}

        private void Measures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<MeasureModel> _mm = ((ObservableCollection<MeasureModel>)sender);
            LineSeries _ls = Series.Single(x => x.Item3.Equals(_mm)).Item1;
            foreach (object _m in e.NewItems)
            {
                _ls.Points.Add(new DataPoint(DateTimeAxis.ToDouble(((MeasureModel)_m).Time), ((MeasureModel)_m).Value));
            }
            RaisePropertyChanged("LineSeries");
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Model.InvalidatePlot(true);
            });
        }
        #endregion  
    }

    public class ChartListModel : BaseModel, IEnumerable
    {
        #region Members
        ObservableCollection<ChartModel> _charts;
        ChartModel _current;
        ObservableCollection<WorkerModel> _workers;
        #endregion

        #region Construction
        public ChartListModel()
        {
            _charts = new ObservableCollection<ChartModel>();
            Navigation = new ObservableCollection<BaseModel>();
            _current = new ChartModel();
            Chart.Name = "New Chart";
            Chart.Command = new RelayCommand(param => SharedData._command(SharedData._chartList.Chart), param => this.CanExe);
            Workers = SharedData._workerList.Workers;
        }
        #endregion

        private bool CanExe
        {
            get { return true; }
        }

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
                RaisePropertyChanged("Workers");
            }
        }

        public ChartModel Chart
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
                RaisePropertyChanged("Chart");
            }
        }

        public ObservableCollection<ChartModel> Charts
        {
            get
            {
                return _charts;
            }
            set
            {
                _charts = value;
                RaisePropertyChanged("Charts");
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
            return new ListEnum<ChartModel>(_charts);
        }
        #endregion
    }
}
