using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Net;
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_GUI.UserControls
{
    /// <summary>
    /// Interaction logic for HostList.xaml
    /// </summary>
    public partial class HostList : UserControl
    {
        public HostList()
        {
            InitializeComponent();
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HostModel h = (HostModel)List.SelectedItem;
                NewHostBox.Text = "["+h.IP.ToString()+"]";
                NewRateBox.Text = (h.Rate/1000).ToString();
            }
            catch (Exception)
            {

            }  
        }

        private void AddHost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HostModel host = SharedData._hostList.Hosts.SingleOrDefault(h => h.IP.Equals(IPAddress.Parse(NewHostBox.Text)));
                if (host == null)
                {
                    host = new HostModel();
                    host.IP = IPAddress.Parse(NewHostBox.Text);
                    host.Rate = Convert.ToInt32(NewRateBox.Text) * 1000;
                    createWorkers(host);
                    SharedData._hostList.Hosts.Add(host);
                    NewHostBox.Text = "";
                }
                else
                {
                    host.IP = IPAddress.Parse(NewHostBox.Text);
                    host.Rate = Convert.ToInt32(NewRateBox.Text) * 1000;
                    foreach(WorkerModel _wm in SharedData._workerList.Workers.ToList().FindAll(h => h.IP.Equals(host.IP)))
                    {
                        _wm.Rate = host.Rate;
                        _wm.IP = host.IP;
                    }
                    NewHostBox.Text = "";
                }
            }
            catch (Exception)
            {

            }
        }

        private void ClearHost_Click(object sender, RoutedEventArgs e)
        {
            NewHostBox.Text = "";
            NewRateBox.Text = "";
        }

        private void createWorkers(HostModel hwm)
        {
            WorkerListModel _wlm = new WorkerListModel();
            _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Temp,         hwm.Rate, 0));
            _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Vcc3,         hwm.Rate, 0));
            _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Ping,         hwm.Rate, 0));
            _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Humidity,     hwm.Rate, 0));
            _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Light,        hwm.Rate, 0));
            _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Troughput,    hwm.Rate, 1024));
            int i = 0;
            _wlm.Workers.OrderBy(x => x.Worker.MethodToRun.Method.Name);
            foreach (WorkerModel w in _wlm.Workers)
            {
                w.Worker._startTime = (w.Worker.Rate / _wlm.Workers.Count) * i++;
                Thread t = new Thread(w.Worker.Work);
                t.IsBackground = true;
                SharedData._threads.Add(t);
                t.Start();
                if (SharedData._running)
                {
                    w.Worker.Run();
                }
            }
            _wlm.Workers.ToList().ForEach(x => SharedData._workerList.Workers.Add(x));
            hwm.Host.Running = false;
        }

        private void removeWorkers(HostModel hwm)
        {
            List<WorkerModel> _wl = SharedData._workerList.Workers.ToList().FindAll(x => x.Worker.Host.IP.Equals(hwm.IP));
            foreach (WorkerModel _w in _wl)
            {
                _w.Worker.Stop();
                _w.Worker.Host.Running = false;
                SharedData._removedWorkers.Add(_w.Worker);
                SharedData._workerList.Workers.Remove(_w);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HostModel host = SharedData._hostList.Hosts.SingleOrDefault(h => h.IP.Equals(((HostModel)List.SelectedItem).IP));
                SharedData._hostList.Hosts.Remove(host);
                removeWorkers(host);
            }
            catch (Exception)
            {

            }
        }

        private void Start_Stop_Click(object sender, RoutedEventArgs e)
        {
            try{
                HostModel _host = SharedData._hostList.Hosts.SingleOrDefault(h => h.IP.Equals(((HostModel)List.SelectedItem).IP));               
                foreach (WorkerModel _wm in SharedData._workerList)
                {
                    if (_wm.IP.Equals(_host.IP))
                    {
                       
                        if (_host.Running){    
                            _wm.Worker.Pause();
                        } else {
                            _wm.Worker.Run();
                        }
                    }
                }
                if (_host.Running){
                    _host.Running = false;
                    ((Button)sender).Content = "Start";
                }  else {
                    _host.Running = true;
                    ((Button)sender).Content = "Stop";
                }
            } catch (Exception){

            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List.SelectedItem != null)
            {
                if (((HostModel)List.SelectedItem).Running)
                {
                    Start_Stop.Content = "Stop";
                }
                else
                {
                    Start_Stop.Content = "Start";
                }
            }
        }

        private void List_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                Start_Stop_Click(Start_Stop, new RoutedEventArgs());
            }
        }
    }
}
