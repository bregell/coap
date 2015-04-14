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
using CoAP_Analyzer_Client;

namespace CoAp_Analyzer_GUI
{
    /// <summary>
    /// Interaction logic for HostList.xaml
    /// </summary>
    public partial class HostList : UserControl
    {
        public HostList()
        {
            InitializeComponent();

            /*SharedData._hostList.Hosts = new ObservableCollection<HostViewModel>{
                new HostViewModel(IPAddress.Parse("[aaaa::212:4b00:60d:9abb]")), 
                new HostViewModel(IPAddress.Parse("[aaaa::212:4b00:60d:9ac3]")),
                new HostViewModel(IPAddress.Parse("[aaaa::212:4b00:60d:9b57]")), 
                new HostViewModel(IPAddress.Parse("[aaaa::212:4b00:60d:9b59]"))
            };*/

            this.DataContext = SharedData._hostList;
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                HostViewModel h = (HostViewModel)List.SelectedItem;
                NewHostBox.Text = h.IP.ToString();
                NewRateBox.Text = h.Rate.ToString();
                AddHost.Content = "Remove";

            }
            catch (Exception)
            {

            }  
        }

        private void AddHost_Click(object sender, RoutedEventArgs e)
        {
            if (AddHost.Content.ToString() == "Add Host")
            {
                try
                {
                    HostViewModel host = SharedData._hostList.Hosts.FirstOrDefault(h => h.IP == IPAddress.Parse(NewHostBox.Text));
                    if (host == null)
                    {
                        host = new HostViewModel();
                        host.IP = IPAddress.Parse(NewHostBox.Text);
                        host.Rate = Convert.ToInt32(NewRateBox.Text) * 1000;
                        createWorkers(host);
                        SharedData._hostList.Hosts.Add(host);
                        NewHostBox.Text = "";
                    }
                }
                catch (Exception)
                {

                }
            }
            else if (AddHost.Content.ToString() == "Remove")
            {
                try
                {
                    HostViewModel host = SharedData._hostList.Hosts.First(_host => _host.IP.ToString() == NewHostBox.Text);
                    SharedData._hostList.Hosts.Remove(host);
                    removeWorkers(host);
                    NewHostBox.Text = "";
                    NewRateBox.Text = "";
                    AddHost.Content = "Add Host";
                }
                catch (Exception)
                {

                }
            }
        }

        private void ClearHost_Click(object sender, RoutedEventArgs e)
        {
            NewHostBox.Text = "";
            NewRateBox.Text = "";
            AddHost.Content = "Add Host";
        }

        private void createWorkers(HostViewModel hwm)
        {
            SharedData._workerList.Workers.Add(new WorkerModel(new Worker(hwm.Host, hwm.Host.Temp, hwm.Rate, 0)));
            SharedData._workerList.Workers.Add(new WorkerModel(new Worker(hwm.Host, hwm.Host.Vcc3, hwm.Rate, 0)));
            SharedData._workerList.Workers.Add(new WorkerModel(new Worker(hwm.Host, hwm.Host.Ping, hwm.Rate, 0)));
            SharedData._workerList.Workers.Add(new WorkerModel(new Worker(hwm.Host, hwm.Host.Humidity, hwm.Rate, 0)));
            SharedData._workerList.Workers.Add(new WorkerModel(new Worker(hwm.Host, hwm.Host.Light, hwm.Rate, 0)));
            SharedData._workerList.Workers.Add(new WorkerModel(new Worker(hwm.Host, hwm.Host.Troughput, hwm.Rate, 1024)));
            int i = 0;
            SharedData._workerList.Workers.OrderBy(x => x.Worker._methodToRun.Method.Name);
            foreach (WorkerModel w in SharedData._workerList.Workers)
            {
                w.Worker._startTime = (w.Worker._rate / SharedData._workerList.Workers.Count) * i++;
                Thread t = new Thread(w.Worker.Work);
                t.IsBackground = true;
                SharedData._threads.Add(t);
                t.Start();
                if (SharedData._running)
                {
                    w.Worker.Run();
                }
            }
        }

        private void removeWorkers(HostViewModel hwm)
        {
            List<WorkerModel> _wl = SharedData._workerList.Workers.ToList().FindAll(x => x.Worker._host.IP.ToString() == hwm.IP.ToString());
            foreach (WorkerModel _w in _wl)
            {
                _w.Worker.Stop();
                SharedData._removedWorkers.Add(_w.Worker);
                SharedData._workerList.Workers.Remove(_w);
            }
        }
    }
}
