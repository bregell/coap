﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.ComponentModel;
using CoAP_Analyzer_Client;
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
            //this.DataContext = SharedData._chartList[0];  
        }
    }
    //SharedData._measureList.Measures.Clear();
    //        foreach (MeasureModel _m in w.Worker.Measures)
    //        {
    //            SharedData._measureList.Measures.Add(_m);
    //        }
    //public class TestModel : INotifyPropertyChanged
    //{
    //    LineSeries _ls;
    //    PlotModel _pm;

    //    public TestModel()
    //    {
    //        PlotModel _pm = new PlotModel { Title = "Test Plot" };
    //        _pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Unit = "Time" });
    //        _pm.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Unit = "Unit",   });
    //        _ls = new LineSeries { DataFieldX = "Time", DataFieldY = "Value" };
    //        _ls.Title = "Test Title";
    //        Series.ItemsSource = SharedData._measureList.Measures;
    //        _pm.Series.Add(_ls);
    //        Model = _pm;
    //    }


    //    public PlotModel Model 
    //    { 
    //        get 
    //        { 
    //            return _pm; 
    //        } 
    //        set 
    //        { 
    //            _pm = value; 
    //            RaisePropertyChanged("Model"); 
    //        } 
    //    }

    //    public LineSeries Series 
    //    { 
    //        get 
    //        { 
    //            return _ls; 
    //        } 
    //        set { 
    //            _ls = value;
    //            RaisePropertyChanged("Series");
    //        } 
    //    }

    //    #region INotifyPropertyChanged Members
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    #endregion

    //    #region Methods
    //    private void RaisePropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //    #endregion
    //}
}