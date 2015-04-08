using System;
using System.Data;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using CoAP.Server;
using CoAP;
using CoAP.Net;
using CoAP.Stack;
using CoAP.Util;
using System.Threading;
using ExcelLibrary;

namespace CoAP_Analyzer_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>{};
            List<Worker> workers = new List<Worker>{};         
            List<Host> hosts = new List<Host>{
                //new Host("[aaaa::212:4b00:60d:9abb]")//,
                //new Host("[aaaa::212:4b00:60d:9b57]"),
                //new Host("[aaaa::212:4b00:60d:9b59]"),
                new Host("[aaaa::212:4b00:60d:9ac3]")
                //new Host("[::1]")
            };
            foreach(Host h in hosts){
                Worker w = new Worker();
                w.h = h;
                w.rate = 5000;
                workers.Add(w);
                w.methodToRun = w.h.Temp;
                Thread t = new Thread(w.Work);
                threads.Add(t);
                t.Start();
            }
            Thread.Sleep(3 * 5000);
            System.Data.DataSet ds = new System.Data.DataSet();
            List<DataTable> _tables = new List<DataTable>();
            List<String> _tablenames = new List<String>{"Ping", "Troughput", "Hops", "Temp", "Light", "Humidity"};
            foreach (String s in _tablenames)
            {
                DataTable _table = new DataTable(s);
                _table.Columns.Add(new System.Data.DataColumn("Ip"));
                _table.Columns.Add(new System.Data.DataColumn(s));
                _table.Columns.Add(new System.Data.DataColumn("Time"));
                _tables.Add(_table);

            }
            foreach(Worker w in workers){
                w.Stop();
                while (!w._done) ;
                foreach(List<Measure> lm in w.h.getMeasures()){
                    
                }
                workers.Remove(w);
            }
            Thread.Sleep(3 * 5000);
            ExcelLibrary.DataSetHelper.CreateWorkbook("Output.xls", ds);
        }
    }

    private class Temp
    {
        public double temp;
        public string unit; 
    }

    private class Humididy
    {
        public double humidity;
        public string unit; 
    }

    private class Light
    {
        public double light;
        public string unit; 
    }

    private class Vcc3
    {
        public double voltage;
        public string unit; 

    }

    private class Measure
    {
        public double value;
        public DateTime time;

        public Measure(double v, DateTime t)
        {
            value = v;
            time = t;
        }
    }
}
