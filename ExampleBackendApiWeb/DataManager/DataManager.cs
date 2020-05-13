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
        public IResourceRepositoryIntId resourceRepository { get; }

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
            resourceRepository = new ResourceRepositoryIntId(initialData);
        }
    }
}
