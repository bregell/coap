using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoAP.Server;
using CoAP;
using CoAP.Net;
using CoAP.Stack;
using CoAP.Util;
using System.Threading;
using CoAP_Analyzer_Server.Resources;

namespace CoAP_Analyzer_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            CoapConfig _config = new CoapConfig();
            _config.DefaultBlockSize = 32;
            _config.MaxMessageSize = 32;

            CoapServer server = new CoapServer(_config);
            CoAPEndPoint endpoint = new CoAPEndPoint(CoapConstants.DefaultPort, _config);
            server.AddEndPoint(endpoint);

            server.Add(new Temp("/sensors/temp"));
            server.Add(new Humidity("/sensors/humidity"));
            server.Add(new Vcc3("/sensors/vcc3"));
            server.Add(new Storage("/data/buffer"));
            server.Add(new Light("/sensors/light"));
            server.Add(new Hops("/info/hops"));

            try
            {
                server.Start();

                Console.Write("CoAP server [{0}] is listening on", server.Config.Version);

                foreach (var item in server.EndPoints)
                {
                    Console.Write(" ");
                    Console.Write(item.LocalEndPoint);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
