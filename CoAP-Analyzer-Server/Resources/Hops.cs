using System;
using System.Text;
using CoAP.Server.Resources;
using CoAP;

namespace CoAP_Analyzer_Server.Resources
{
    class Hops : Resource
    {
        Random rnd = new Random();
        public Hops(String name)
            : base(name)
        {
            Attributes.Title = "Hops";
            rnd.Next(1, 5);
        }

        protected override void DoGet(CoapExchange exchange)
        {
            Response res = new Response(StatusCode.Content);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"hops\":");
            sb.Append(rnd.ToString());
            sb.Append("}");
            res.Payload = Encoding.UTF8.GetBytes(sb.ToString());
            res.ContentType = CoAP.MediaType.ApplicationJson;
            exchange.Respond(res);
        }
    }
}
