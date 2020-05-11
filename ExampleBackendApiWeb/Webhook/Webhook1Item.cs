using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Webhook
{
    /// <summary>
    /// информация передающаяся в Webhook1
    /// </summary>
    public class Webhook1Item
    {
        public int LastChangedId;
        public string LastChangedValue;
        public string LastChangedEventType;

        public Webhook1Item(int lastChangedId, string lastChangedValue, string lastChangedEventType)
        {
            LastChangedId = lastChangedId;
            LastChangedValue = lastChangedValue;
            LastChangedEventType = lastChangedEventType;
        }
    }
}
