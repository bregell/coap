#define DEBUG
using CoAP;
using CoAP.Net;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

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
        public Measure Resource(Resource _res)
        {
            _res.Timeout = (_res.Timeout == 0) ? System.Threading.Timeout.Infinite : _res.Timeout;
            //Prepare the package
            Request _req;
            Uri uri = new UriBuilder(CoapConstants.UriScheme, _ip.ToString(), CoapConstants.DefaultPort, _res.Path).Uri;
            

            CoapClient _cc = new CoapClient(uri, _conf);
            _cc.EndPoint = _endpoint;
            _cc.Timeout = _res.Timeout;


            if (_res.Path.Equals(""))
            {
                _req = new Request(Code.Empty, true);
                _req.URI = uri;
                _req.Token = CoapConstants.EmptyToken;
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                _req.Response = _cc.Send(_req);
                stopWatch.Stop();
                if (_req.Response == null)
                {
                    return new Measure(stopWatch.ElapsedMilliseconds, "ms");
                }
                else if (_req.IsTimedOut)
                {
                    return new Measure(-1, "Timeout");
                }
            } else if (_res.Param > 0){
                _req = new Request(Method.PUT);
                //_req.Timeout += delegate(object sender, EventArgs e)
                //{
                //    _req.IsTimedOut = true;
                //};
                _req.URI = uri;
                String payload = new String('X', _res.Param);
                _req.SetPayload(payload, MediaType.TextPlain);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                _req.Response = _cc.Send(_req);
                stopWatch.Stop();
                if (_req.Response != null)
                {
                    Debug.WriteLine("#####");
                    Debug.WriteLine(stopWatch.ElapsedMilliseconds.ToString());
                    Debug.WriteLine(_req.Response.RTT);
                    Debug.WriteLine("#####");
                    return new Measure((((double)_res.Param / 1024.0) / (_req.Response.RTT / 1000.0)), "kB/s");
                }
                else if (_req.IsTimedOut)
                {
                    return new Measure(-1, "Timeout");
                }
            } else {
                //Send Package 
                _req = new Request(Method.GET);
                //_req.Timeout += delegate(object sender, EventArgs e)
                //{
                //    _req.IsTimedOut = true;
                //};
                _req.URI = uri;
                _req.Accept = MediaType.ApplicationJson;
                _req.Response = _cc.Send(_req);
                if (_req.Response != null && _req.Response.PayloadSize != 0)
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Measure));
                    try
                    {
                        MemoryStream stream1 = new MemoryStream(_req.Response.Payload);
                        Measure t = (Measure)ser.ReadObject(stream1);
                        t.Time = DateTime.Now;
                        return t;
                    }
                    catch (Exception)
                    {
                        return new Measure(-1, "Serialization Error");
                    }
                }
                else if (_req.IsTimedOut)
                {
                    return new Measure(-1, "Timeout");
                }
            }
            return new Measure(-1, "Error");
        }

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
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TempC));
                try {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    TempC t = (TempC)ser.ReadObject(stream1);
                    t.Time = DateTime.Now;

                    return t;
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
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LightC));
                try {    
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    LightC l = (LightC)ser.ReadObject(stream1);
                    return new Measure(l.Light, l.Unit, DateTime.Now);
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
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(HumididyC));
                try {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    HumididyC h = (HumididyC)ser.ReadObject(stream1);
                    return new Measure(h.Humidity, h.Unit, DateTime.Now);
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
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(VoltageC));
                try
                {
                    MemoryStream stream1 = new MemoryStream(req.Response.Payload);
                    VoltageC v = (VoltageC)ser.ReadObject(stream1);
                    return new Measure(v.Voltage, v.Unit, DateTime.Now);
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
