﻿using CoAP_Analyzer_Client.Models;
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
        Visibility _controls;
        #endregion

        #region Construction
        public ChartModel()
        {
            Model = new PlotModel();
            Model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Unit = "Time" });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Unit", });
            Model.Title = "New Plot";
            Series = new ObservableCollection<Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>>>();
            Controls = Visibility.Collapsed;
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

        public Visibility Controls 
        {
            get { return _controls; }
            set { _controls = value; RaisePropertyChanged("Controls"); } 
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
            RaisePropertyChanged("LineSeries");
            RefreshPlot();
        }

        public void RemoveSeries(WorkerModel _wm)
        {
            Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>> _tp;
            try
            {
                _tp = Series.Single(x => x.Item3.Equals(_wm.Measure));
                _tp.Item3.CollectionChanged -= _tp.Item2;
                Model.Series.Remove(_tp.Item1);
                Series.Remove(_tp);
                RaisePropertyChanged("LineSeries");
                RefreshPlot();
            }
            catch (Exception)
            {

            }
        }

        /**
         * 
         **/
        public void ClearSeries()
        {
            Model.Series.Clear();
            Series.Clear();
            RaisePropertyChanged("LineSeries");
            RefreshPlot();
        }

        private void Measures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<MeasureModel> _mm = ((ObservableCollection<MeasureModel>)sender);
            LineSeries _ls = Series.Single(x => x.Item3.Equals(_mm)).Item1;
            foreach (object _m in e.NewItems)
            {
                _ls.Points.Add(new DataPoint(DateTimeAxis.ToDouble(((MeasureModel)_m).Time), ((MeasureModel)_m).Value));
            }
            RaisePropertyChanged("LineSeries");
            RefreshPlot();
        }

        public void RefreshPlot()
        {
            Application.Current.Dispatcher.InvokeAsync((Action)delegate
            {
                Model.InvalidatePlot(true);
            });
        }
        #endregion  
    }
}
