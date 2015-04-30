using CoAP_Analyzer_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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

        //private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        HostModel h = (HostModel)List.SelectedItem;
        //        NewHostBox.Text = "["+h.IP.ToString()+"]";
        //        NewRateBox.Text = (h.Rate/1000).ToString();
        //    }
        //    catch (Exception)
        //    {

        //    }  
        //}

        private void AddHost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HostModel host = SharedData._hostList.Hosts.SingleOrDefault(h => h.IP.Equals(IPAddress.Parse(node_ip.Text)));
                if (host == null)
                {
                    host = new HostModel();
                    host.IP = IPAddress.Parse(node_ip.Text);
                    host.Name = node_name.Text;
                    //createWorkers(host);
                    SharedData._hostList.Hosts.Add(host);
                    node_ip.Text = "[Node::IP]";
                    node_name.Text = "Node Name";
                    host.Workers.Workers.CollectionChanged += Workers_CollectionChanged;
                }
            }
            catch (Exception)
            {

            }
        }

        void Workers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object _m in e.NewItems)
                {
                    SharedData._workerList.Workers.Add((WorkerModel)_m);
                }
            }
            if (e.OldItems != null)
            {
                foreach (object _m in e.OldItems)
                {
                    SharedData._workerList.Workers.Remove((WorkerModel)_m);
                }
            }
        }

        private void ClearHost_Click(object sender, RoutedEventArgs e)
        {
            node_ip.Text = "[Node::IP]";
            node_name.Text = "Node Name";
        }

        //private void createWorkers(HostModel hwm)
        //{
        //    WorkerListModel _wlm = new WorkerListModel();
        //    _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Resource, hwm.Rate, new Resource(10000, 0, "sensors/temp", "Temp")));
        //    _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Resource, hwm.Rate, new Resource(10000, 0, "sensors/humidity", "Humidity")));
        //    _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Resource, hwm.Rate, new Resource(10000, 0, "sensors/light", "Light")));
        //    _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Resource, hwm.Rate, new Resource(10000, 0, "sensors/vdd3", "Voltage")));
        //    _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Resource, hwm.Rate, new Resource(10000, 0, "", "Ping")));
        //    _wlm.Workers.Add(new WorkerModel(hwm.Host, hwm.Host.Resource, hwm.Rate, new Resource(hwm.Rate, 1024, "data/buffer", "Troughput")));
        //    int i = 0;
        //    _wlm.Workers.OrderBy(x => x.Worker.MethodToRun.Method.Name);
        //    foreach (WorkerModel w in _wlm.Workers)
        //    {
        //        w.Worker._startTime = (w.Worker.Rate / _wlm.Workers.Count) * i++;
        //        Thread t = new Thread(w.Worker.Work);
        //        t.IsBackground = true;
        //        SharedData._threads.Add(t);
        //        t.Start();
        //        if (SharedData._running)
        //        {
        //            w.Worker.Run();
        //        }
        //    }
        //    _wlm.Workers.ToList().ForEach(x => SharedData._workerList.Workers.Add(x));
        //    hwm.Host.Running = false;
        //}

        private void removeWorkers(HostModel hwm)
        {
            List<WorkerModel> _wl = SharedData._workerList.Workers.ToList().FindAll(x => x.Worker.Host.IP.Equals(hwm.IP));
            foreach (WorkerModel _w in _wl)
            {
                _w.Worker.Stop();
                _w.Worker.Host.Running = false;
                SharedData._workerList.Workers.Remove(_w);
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HostModel host = SharedData._hostList.Hosts.SingleOrDefault(h => h.IP.Equals(((HostModel)host_list.SelectedItem).IP));
                SharedData._hostList.Hosts.Remove(host);
                removeWorkers(host);
            }
            catch (Exception)
            {

            }
        }

        //private void Start_Stop_Click(object sender, RoutedEventArgs e)
        //{
        //    try{
        //        HostModel _host = SharedData._hostList.Hosts.SingleOrDefault(h => h.IP.Equals(((HostModel)List.SelectedItem).IP));               
        //        foreach (WorkerModel _wm in SharedData._workerList)
        //        {
        //            if (_wm.IP.Equals(_host.IP))
        //            {
                       
        //                if (_host.Running){    
        //                    _wm.Worker.Pause();
        //                } else {
        //                    _wm.Worker.Run();
        //                }
        //            }
        //        }
        //        if (_host.Running){
        //            _host.Running = false;
        //            ((Button)sender).Content = "Start";
        //        }  else {
        //            _host.Running = true;
        //            ((Button)sender).Content = "Stop";
        //        }
        //    } catch (Exception){

        //    }
        //}

        private void list_selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            if (host_list.SelectedItem != null)
            {
                HostModel _hm = (HostModel)host_list.SelectedItem;        
                SharedData._hostCreate.Host = _hm;
                SharedData._hostCreate.Workers = _hm.Workers;
                _hm.Workers.Enabled = true;
            }
        }

        //private void List_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key.Equals(Key.Enter))
        //    {
        //        Start_Stop_Click(sender, new RoutedEventArgs());
        //    }
        //}
    }
}
