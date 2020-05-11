using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Webhook
{
    /// <summary>
    /// параметр для запуска потока отправляющего Webhook1
    /// </summary>
    public class Webhook1StartParam
    {
        public System.Net.Http.HttpClient client;
        public Webhook1Item item;

        public Webhook1StartParam(HttpClient client, Webhook1Item item)
        {
            this.client = client;
            this.item = item;
        }
    }
}
