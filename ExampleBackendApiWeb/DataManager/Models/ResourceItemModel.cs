using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.DataManager.Models
{
    public class ResourceItemModel<T>
    {
        public T id { set; get; }
        public string value { set; get; }
        public ResourceItemModel(T id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
