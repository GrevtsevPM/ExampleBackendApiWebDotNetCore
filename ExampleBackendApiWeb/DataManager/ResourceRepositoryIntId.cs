using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleBackendApiWeb.Enums;

namespace ExampleBackendApiWeb.DataManager
{
    public class ResourceRepositoryIntId : ResourceRepository<int>, IResourceRepository<int>, IResourceRepositoryIntId
    {
        public ResourceRepositoryIntId(Dictionary<int, string> initialResources = null) : base(initialResources)
        {
        }

        public int Create(out ModifyDataResultEnum operationResult, string value = "")
        {
            var newId = Resources.Keys.Count > 0 ? Resources.Keys.Max() + 1 : 1;
            operationResult = Create(newId, value);
            return newId;
        }
    }
}
