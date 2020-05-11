using ExampleBackendApiWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.DataManager
{
    public class ModifyDataResultHelper
    {
        public static bool IsResultError(ModifyDataResultEnum result)
        {
            return (int)result < 1;
        }
    }
}
