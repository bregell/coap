using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using CoAP.Server;
using CoAP;
using CoAP.Net;
using CoAP.Stack;
using CoAP.Util;
using System.Threading;
using ExcelLibrary;

namespace CoAP_Analyzer_CLI
{
    public class Host
    {
        public IPAddress ip;
        public List<Measure> ping;
        public List<Measure> troughput;
        public List<Measure> temp;
        public List<Measure> humidity;
        public List<Measure> light;
        public List<Measure> vcc3;

        private CoapConfig _conf;
        private IEndPoint _endpoint;

        public int hops;

        public Host(string ip)
        {
            this.ip = IPAddress.Parse(ip);
            hops = 0;

            _conf = new CoapConfig();
            _conf.DefaultBlockSize = 32;
            _conf.MaxMessageSize = 32;
            _endpoint = new CoAPEndPoint(_conf);
            _endpoint.Start();

            ping = new List<Measure>();
            troughput = new List<Measure>();
            light = new List<Measure>();
            temp = new List<Measure>();
            humidity = new List<Measure>();
            vcc3 = new List<Measure>();

        }

        public List<List<Measure>> getMeasures()
        {
            return new List<List<Measure>> { vcc3, ping, troughput, temp, light, humidity };
        }

        public DataTable fillDataTable(DataTable dt, List<Measure> lm)
        {
            foreach (Measure m in lm)
            {
                DataRow r = dt.NewRow();
                r["Ip"] = ip.ToString();
                r[dt.Columns[1]] = dt.Columns[1];
                r["Time"] = m.time;
                dt.Rows.Add(r);
            }
            return dt;
        }

        public void Temp(int timeout)
        {
            timeout = (timeout == null) ? System.Threading.Timeout.Infinite : timeout;
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/temp").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.WaitForResponse(timeout);
            if (req.Response != null)
            {
                if (req.Response.PayloadSize != 0)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Temp));
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Temp t = (Temp)ser.ReadObject(stream1);
                    temp.Add(new Measure(t.temp, DateTime.Now));
                }
                else
                {
                    temp.Add(new Measure(-1, DateTime.Now));
                }
            }
            else
            {
                temp.Add(new Measure(-1, DateTime.Now));
            }
        }

        public void Light()
        {
            //Prepare the package
            Request req = new Request(Method.PUT);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/light").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Respond += delegate(object sender, ResponseEventArgs e)
            {
                if (e.Response != null)
                {
                    if (e.Response.PayloadSize != 0)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Light));
                        MemoryStream stream1 = new MemoryStream(e.Response.Payload);
                        Light l = (Light)ser.ReadObject(stream1);
                        light.Add(new Measure(l.light, DateTime.Now));
                    }
                    else
                    {
                        light.Add(new Measure(-1, DateTime.Now));
                    }
                }
                else
                {
                    light.Add(new Measure(-1, DateTime.Now));
                }
            };
        }

        public void Humidity()
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/humidity").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Respond += delegate(object sender, ResponseEventArgs e)
            {
                if (e.Response != null)
                {
                    if (e.Response.PayloadSize != 0)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Humididy));
                        MemoryStream stream1 = new MemoryStream(e.Response.Payload);
                        Humididy h = (Humididy)ser.ReadObject(stream1);
                        humidity.Add(new Measure(h.humidity, DateTime.Now));
                    }
                    else
                    {
                        humidity.Add(new Measure(-1, DateTime.Now));
                    }
                }
                else
                {
                    humidity.Add(new Measure(-1, DateTime.Now));
                }
            };
        }

        public void Vcc3()
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            req.URI = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "sensors/humidity").Uri;

            //Send Package
            req.Send(_endpoint);
            req.Respond += delegate(object sender, ResponseEventArgs e)
            {
                if (e.Response != null)
                {
                    if (e.Response.PayloadSize != 0)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Vcc3));
                        MemoryStream stream1 = new MemoryStream(e.Response.Payload);
                        Vcc3 v = (Vcc3)ser.ReadObject(stream1);
                        vcc3.Add(new Measure(v.voltage, DateTime.Now));
                    }
                    else
                    {
                        vcc3.Add(new Measure(-1, DateTime.Now));
                    }
                }
                else
                {
                    vcc3.Add(new Measure(-1, DateTime.Now));
                }
            };
        }

        public System.Data.DataTable getPing(System.Data.DataTable tablePing)
        {
            foreach (Measure t in ping)
            {
                System.Data.DataRow row = tablePing.NewRow();
                row[0] = ip;
                row[1] = t.value;
                row[2] = hops;
                tablePing.Rows.Add(row);
            }
            return tablePing;
        }

        public System.Data.DataTable getTroughput(System.Data.DataTable tableTroughput)
        {
            foreach (Measure t in troughput)
            {
                System.Data.DataRow row = tableTroughput.NewRow();
                row[0] = ip;
                row[1] = t.value;
                row[2] = hops;
                tableTroughput.Rows.Add(row);
            }
            return tableTroughput;
        }

        public void getHops()
        {
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "info/hops").Uri;
            req.URI = uri;

            req.Send(_endpoint);
            req.Respond += delegate(object sender, ResponseEventArgs e)
            {
                hops = Convert.ToInt32(e.Response.Payload.ToString());
            };
        }

        public void Ping()
        {
            _conf.DefaultBlockSize = 32;
            _conf.MaxMessageSize = 32;

            IEndPoint _endpoint;
            _endpoint = new CoAPEndPoint(_conf);
            _endpoint.Start();

            Request req = new Request(Method.PUT);
            req.SetPayload(String.Empty, MediaType.TextPlain);
            req.URI = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort).Uri;

            req.Send(_endpoint);
            req.Respond += delegate(object sender, ResponseEventArgs e)
            {
                if (e.Response != null)
                {
                    ping.Add(new Measure(e.Response.RTT, DateTime.Now));
                }
                else
                {
                    ping.Add(new Measure(-1, DateTime.Now));
                }
            };
        }

        public void Troughput(int bytes)
        {
            //Prepare the package
            String payload = new String('X', bytes);
            Request req = new Request(Method.PUT);
            req.SetPayload(payload, MediaType.TextPlain);
            req.URI = new UriBuilder(CoapConstants.UriScheme, ip.ToString(), CoapConstants.DefaultPort, "data/buffer").Uri;

            //Send Package
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            req.Send(_endpoint);
            req.Respond += delegate(object sender, ResponseEventArgs e)
            {
                stopWatch.Stop();
                if (e.Response != null)
                {
                    troughput.Add(new Measure((double)bytes / (double)stopWatch.ElapsedMilliseconds, DateTime.Now));
                }
                else
                {
                    troughput.Add(new Measure(-1, DateTime.Now));
                }
            };
        }
    }
}
