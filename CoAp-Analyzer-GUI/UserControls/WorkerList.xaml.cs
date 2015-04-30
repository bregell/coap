using CoAP_Analyzer_Client;
using CoAP_Analyzer_Client.Models;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoAP_Analyzer_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for WorkerList.xaml
    /// </summary>
    public partial class WorkerList : UserControl
    {
        public WorkerList()
        {
            InitializeComponent();
            DataContextChanged += WorkerList_DataContextChanged;
        }

        private void WorkerList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(DataContext != null)
                ((WorkerListModel)DataContext).PropertyChanged += WorkerList_PropertyChanged;
        }

        void WorkerList_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Enabled")
            {
                content.IsEnabled = ((WorkerListModel)sender).Enabled;
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WorkerModel w = (WorkerModel)(workerlist.SelectedItem);
            try
            {
                SharedData._measureList.Measures = w.Worker.Measures;

            }
            catch (Exception)
            {
                
            }  
        }

        private void has_data_Checked(object sender, RoutedEventArgs e)
        {
            resource_size.Visibility = System.Windows.Visibility.Visible;
            resource_size_label.Visibility = System.Windows.Visibility.Visible;
        }

        private void has_data_Unchecked(object sender, RoutedEventArgs e)
        {
            resource_size.Visibility = System.Windows.Visibility.Hidden;
            resource_size_label.Visibility = System.Windows.Visibility.Hidden;
        }

        private void add_res_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _rate = Convert.ToInt32(resource_rate.Text)*1000;
                int _timeout = Convert.ToInt32(resource_timeout.Text)*1000;
                int _size = 0;
                if (has_data.IsChecked.Value)
                {
                    _size = Convert.ToInt32(resource_size.Text);
                }  
                Resource _res = new Resource(_timeout, _size, resource_path.Text, resource_name.Text, _rate);
                WorkerModel _wm = new WorkerModel(SharedData._hostCreate.Host.Host, SharedData._hostCreate.Host.Host.Resource, _res);
                WorkerListModel _wlm = (WorkerListModel)DataContext;
                if(!SharedData._hostCreate.Resources.Any(x => x.Name == _res.Name))
                {
                    SharedData._hostCreate.Resources.Add(_res);
                }
                if (!_wlm.Workers.Any(x => x.Worker.Resource.Name == _res.Name))
                {
                    _wlm.Workers.Add(_wm);
                    Thread _t = new Thread(_wm.Worker.Work);
                    _t.IsBackground = true;
                    _t.Start();
                    clear_res_Click(sender, e);
                }
            }
            catch (Exception)
            {

            }
        }

        private void clear_res_Click(object sender, RoutedEventArgs e)
        {
            resource_name.Clear();
            resource_path.Clear();
            resource_size.Clear();
            has_data.IsChecked = false;
            resource_rate.Clear();
            resource_timeout.Clear();
        }

        private void worker_start_Click(object sender, RoutedEventArgs e)
        {
            Button _btn = (Button)sender;
            var item = (sender as FrameworkElement).DataContext;
            int index = workerlist.Items.IndexOf(item);
            WorkerModel _wm = (WorkerModel)workerlist.Items[index];
            if(_wm.Worker.Running){
                _wm.Worker.Pause();
                _btn.Content = "Start";
            }
            else
            {
                _wm.Worker.Run();
                _btn.Content = "Stop";
            }
        }

        private void worker_remove_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = workerlist.Items.IndexOf(item);
            WorkerModel _wm = (WorkerModel)workerlist.Items[index];
            WorkerListModel _wlm = (WorkerListModel)DataContext;
            _wlm.Workers.Remove(_wm);
        }

        private void res_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (res_list.SelectedItem != null)
            {
                Resource _res = (Resource)res_list.SelectedItem;
                resource_name.Text = _res.Name;
                resource_path.Text = _res.Path;
                if (_res.Param > 0)
                {
                    resource_size.Text = _res.Param.ToString();
                    has_data.IsChecked = true;
                }
                resource_rate.Text = (_res.Rate/1000).ToString();
                resource_timeout.Text = (_res.Timeout/1000).ToString();
            }
        }

        private void res_rem_Click(object sender, RoutedEventArgs e)
        {
            WorkerListModel _wlm = (WorkerListModel)DataContext;
            Resource _r = (Resource)res_list.SelectedItem;
            SharedData._hostCreate.Resources.Remove(_r);
        }
    }
}
