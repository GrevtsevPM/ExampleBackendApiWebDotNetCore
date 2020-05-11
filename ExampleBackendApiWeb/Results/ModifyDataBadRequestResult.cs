using ExampleBackendApiWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Results
{
    /// <summary>
    /// класс для возврата кода результата операции клиенту ModifyDataResultEnum
    /// </summary>
    public class ModifyDataBadRequestResult
    {
        public ModifyDataResultEnum operationResult { private set; get; }
        public string resultText { private set; get; }

        public ModifyDataBadRequestResult(ModifyDataResultEnum operationResult)
        {
            this.operationResult = operationResult;
            resultText = Enum.GetName(typeof(ModifyDataResultEnum), operationResult);
        }
    }
}
