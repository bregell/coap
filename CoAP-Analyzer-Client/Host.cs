#define DEBUG
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Diagnostics;
using CoAP;
using CoAP.Net;
using CoAP_Analyzer_Client.Models;

namespace CoAP_Analyzer_Client
{
    public class Host
    {
        #region Members
        IPAddress _ip;
        CoapConfig _conf;
        IEndPoint _endpoint;
        #endregion

        #region Properties
        public IPAddress IP
        {
            get
            {
                return this._ip;
            }
            set
            {
                this._ip = value;
            }
        }

        public bool Running { get; set; }
        #endregion

        #region Construction
        public Host(string ip)
        {
            this._ip = IPAddress.Parse(ip);
            _conf = new CoapConfig();
            _conf.DefaultBlockSize = 32;
            _conf.MaxMessageSize = 32;
            _endpoint = new CoAPEndPoint(_conf);
            _endpoint.Start();
            Running = false;
        }

        public Host()
        {
            _conf = new CoapConfig();
            _conf.DefaultBlockSize = 32;
            _conf.MaxMessageSize = 32;
            _endpoint = new CoAPEndPoint(_conf);
            _endpoint.Start();
            Running = false;
        }
        #endregion

        #region Methods
        public Measure Temp(int timeout)
        {
            timeout = (timeout == 0) ? System.Threading.Timeout.Infinite : timeout;
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, "sensors/temp").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Temp));
                try {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Temp t = (Temp)ser.ReadObject(stream1);
                    
                    return new Measure(t.temp, t.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
            }
                return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Light(int timeout)
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, "sensors/light").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Light));
                try {    
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Light l = (Light)ser.ReadObject(stream1);
                    return new Measure(l.light, l.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Humidity(int timeout)
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, "sensors/humidity").Uri;
            req.URI = uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Humididy));
                try {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Humididy h = (Humididy)ser.ReadObject(stream1);
                    return new Measure(h.humidity, h.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Vcc3(int timeout)
        {
            //Prepare the package
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            req.URI = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, "sensors/vdd3").Uri;

            //Send Package
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null && req.Response.PayloadSize != 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Vcc3));
                try
                {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    Vcc3 v = (Vcc3)ser.ReadObject(stream1);
                    return new Measure(v.voltage, v.unit, DateTime.Now);
                }
                catch (Exception)
                {
                    return new Measure(-1, "Serialization Error", DateTime.Now);
                }
                        
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Hops(int timeout)
        {
            Request req = new Request(Method.GET);
            req.Accept = MediaType.ApplicationJson;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, "info/hops").Uri;
            req.URI = uri;

            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null  && req.Response.PayloadSize != 0)
            {
                return new Measure(Convert.ToInt32(req.Response.Payload.ToString()), "Hops", DateTime.Now);
            }
            return new Measure(-1, "Timeout", DateTime.Now);
        }

        public Measure Ping(int timeout)
        {
            Request req = new Request(Method.GET);
            req.URI = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort).Uri;

            req.Send(_endpoint);
            req.Response = req.WaitForResponse(10000);
            if (req.Response != null)
            {
                    return new Measure(req.Response.RTT, "ms", DateTime.Now);
            }
            else
            {
                return new Measure(-1, "Timeout", DateTime.Now);
            }
        }

        public Measure Troughput(int bytes)
        {
            //Prepare the package
            String payload = new String('X', bytes);
            Request req = new Request(Method.PUT);
            req.SetPayload(payload, MediaType.TextPlain);
            req.URI = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, "data/buffer").Uri;

            //Send Package
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            req.Send(_endpoint);
            req.Response = req.WaitForResponse(100000);
            stopWatch.Stop();
            if (req.Response != null){
                return new Measure( ((bytes * 8.0) / stopWatch.ElapsedMilliseconds) * 1000.0  / 1024.0, "kbit/s", DateTime.Now);
            }
            else
            {
                return new Measure(-1, "Timeout", DateTime.Now);
            }
        }
        #endregion
    }   
}
