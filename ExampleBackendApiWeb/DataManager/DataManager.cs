using ExampleBackendApiWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.DataManager
{
    ///<inheritdoc/>
    /// <summary>
    /// класс-декоратор для настройки ResourceRepository и специфического расширения его функционала
    /// </summary>
    public class DataManagerClass : IDataManagerClass
    {
        public IResourceRepository<int> resourceRepository { get; }

        public long blockStamp { set; get; } = 0;

        public DataManagerClass(Dictionary<int, string> initialData = null)
        {
            if (initialData == null)
            {
                initialData = new Dictionary<int, string>();
                initialData.Add(1, "foo");
                initialData.Add(2, "bar");
                initialData.Add(4, "baz");
            }
            resourceRepository = new ResourceRepository<int>(initialData);
        }

        public int CreateWithNewId(string value, out ModifyDataResultEnum operationResult)
        {
            var newId = resourceRepository.Resources.Keys.Count > 0 ? resourceRepository.Resources.Keys.Max() + 1 : 1;
            operationResult = resourceRepository.Create(newId, value);
            return newId;
        }
    }
}
