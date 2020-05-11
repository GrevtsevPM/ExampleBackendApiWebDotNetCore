using ExampleBackendApiWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.ApiParameters.ResourcesManagerController
{
    public class UpdateInsertSubstrParameter
    {
        public string value { set; get; }
        public UpdateResourceCommandTypeEnum updateType { set; get; }
        public int index { set; get; }
    }
}
