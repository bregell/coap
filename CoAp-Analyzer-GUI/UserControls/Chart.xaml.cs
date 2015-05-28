using CoAP_Analyzer_Client.Models;
using CoAP_Analyzer_GUI.Models;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            ChartModel _cm = (ChartModel)DataContext;
            List<ObservableCollection<MeasureModel>> _lmm = new List<ObservableCollection<MeasureModel>>();
            foreach (Tuple<LineSeries, NotifyCollectionChangedEventHandler, ObservableCollection<MeasureModel>> _tuple in _cm.Series)
            {
                _lmm.Add(_tuple.Item3);
            }
            List<string> _name = new List<string>();
            foreach (Series _ec in _cm.Model.Series)
            {
                _name.Add(_ec.Title);
            }
            DataSet _ds = new DataSet(_cm.Name);
            List<DataTable> _tables = new List<DataTable>();
            int i = 0;
            foreach (ObservableCollection<MeasureModel> _mm in _lmm)
            {
                DataTable _table = new DataTable(_name[i++]);
                _table.Columns.Add(new System.Data.DataColumn("Ip"));
                _table.Columns.Add(new System.Data.DataColumn(_mm[0].Unit, System.Type.GetType("System.Double")));
                _table.Columns.Add(new System.Data.DataColumn("Unit"));
                _table.Columns.Add(new System.Data.DataColumn("Time"));  
                foreach (MeasureModel m in _mm)
                {
                    _table.Rows.Add(m.IP, m.Value, m.Unit, m.Time);
                }
                _tables.Add(_table);
            }
            foreach (DataTable t in _tables)
            {
                _ds.Tables.Add(t);
            }
            try
            {
                ExcelLibrary.DataSetHelper.CreateWorkbook(_cm.Name+".xls", _ds);
            }
            catch (Exception)
            {
                Debug.WriteLine("Cannot save to that file, enter new filename!:");
            }
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            ChartModel _cm = (ChartModel)DataContext;
            if (!SharedData._chartTab.CreateModel.Chart.Equals(_cm))
            {
                SharedData._chartTab.CreateModel.Chart = _cm;
            }        
            SharedData._chartTab.Index = 0;
            _cm.Controls = System.Windows.Visibility.Hidden;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            ChartModel _cm = (ChartModel)DataContext;
            SharedData._chartTab.Navigation.Remove(_cm);
            SharedData._chartTab.Index = 0;
        }
    }
}
