using ExampleBackendApiWeb.DataManager.Models;
using ExampleBackendApiWeb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.DataManager
{
    ///<inheritdoc/>
    /// <summary>
    /// класс для хранения и обработки данных "ресурсов". T - тип id
    /// </summary>
    public class ResourceRepository<T> : IResourceRepository<T>
    {
        /// <summary>
        /// ресурсы
        /// </summary>
        Dictionary<T, string> resources;

        public Dictionary<T, string> Resources
        {
            get
            {
                return new Dictionary<T, string>(resources);
            }
        }

        /// <summary>
        /// конструктор, инициализация данных
        /// </summary>
        public ResourceRepository(Dictionary<T, string> initialResources = null)
        {
            if (initialResources != null)
            {
                resources = initialResources;
            }
        }

        public bool SendWebhook { get; set; }

        public T LastChangedId { get; private set; }
        public string LastChangedValue { get; private set; }
        public string LastChangedEventType { get; private set; }

        /// <summary>
        /// сохранение последнего изменения
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newValue">обновлённое значение</param>
        /// <param name="eventType">тип события</param>
        void setLastChange(T id, string newValue, string eventType)
        {
            LastChangedId = id;
            LastChangedValue = newValue;
            LastChangedEventType = eventType;
            SendWebhook = true;
        }

        public IEnumerable<ResourceItemModel<T>> GetAll()
        {
            return resources.Select(r => new ResourceItemModel<T>(r.Key, r.Value));
        }

        public string GetById(T id, out ModifyDataResultEnum operationResult)
        {
            if (resources.ContainsKey(id))
            {
                operationResult = ModifyDataResultEnum.Success;
                return resources[id];
            }
            else
            {
                operationResult = ModifyDataResultEnum.ErrNotFound;
                return null;
            }
        }

        public string GetSubstring(T id, int start, int length, out ModifyDataResultEnum operationResult)
        {
            if (resources.ContainsKey(id))
            {
                var item = resources[id];
                if (item == null)
                {
                    operationResult = ModifyDataResultEnum.ErrStringNull;
                    return null;
                }
                if (start + length > item.Length)
                {
                    operationResult = ModifyDataResultEnum.ErrIndexOutside;
                    return null;
                }

                operationResult = ModifyDataResultEnum.Success;
                return item.Substring(start, length);
            }
            else
            {
                operationResult = ModifyDataResultEnum.ErrNotFound;
            }
            return null;
        }

        public ModifyDataResultEnum Create(T newId, string value = "")
        {
            if (resources.ContainsKey(newId)) return ModifyDataResultEnum.ErrConflict;
            resources.Add(newId, value);
            setLastChange(newId, value, nameof(Create));

            return ModifyDataResultEnum.Success;
        }

        public ModifyDataResultEnum ReplaceItem(T id, string value)
        {
            if (resources.ContainsKey(id))
            {
                resources[id] = value;
                setLastChange(id, value, nameof(ReplaceItem));
                return ModifyDataResultEnum.Success;
            }
            else
            {
                return ModifyDataResultEnum.ErrNotFound;
            }
        }

        public ModifyDataResultEnum Delete(T id)
        {
            if (resources.ContainsKey(id))
            {
                resources.Remove(id);
                setLastChange(id, "", nameof(Delete));
                return ModifyDataResultEnum.Success;
            }
            else
            {
                return ModifyDataResultEnum.ErrNotFound;
            }
        }

        public string UpdateInsertSubstr(T id, string value, UpdateResourceCommandTypeEnum updateType, out ModifyDataResultEnum operationResult, int index = -1)
        {
            if (!resources.ContainsKey(id))
            {
                operationResult = ModifyDataResultEnum.ErrNotFound;
                return null;
            }
            var item = resources[id];
            if (item == null)
            {
                operationResult = ModifyDataResultEnum.ErrStringNull;
                return null;
            }

            string resultVal;
            switch (updateType)
            {
                case UpdateResourceCommandTypeEnum.InsertAtStart:
                    resultVal = item.Insert(0, value);
                    break;

                case UpdateResourceCommandTypeEnum.InsertAtEnd:
                    resultVal = string.Concat(item, value);
                    break;

                case UpdateResourceCommandTypeEnum.InsertAtIndex:
                    if (index < 0 || index >= item.Length)
                    {
                        operationResult = ModifyDataResultEnum.ErrIndexOutside;
                        return null;
                    }

                    resultVal = item.Insert(index, value);
                    break;
                default:
                    operationResult = ModifyDataResultEnum.ErrWrongParams;
                    return null;
            }
            resources[id] = resultVal;

            setLastChange(id, resultVal, string.Concat(nameof(UpdateInsertSubstr), " ", Enum.GetName(typeof(UpdateResourceCommandTypeEnum), updateType)));
            operationResult = ModifyDataResultEnum.Success;
            return resultVal;
        }

        public string UpdateRemoveSubstr(T id, int index, int length, out ModifyDataResultEnum operationResult)
        {
            if (!resources.ContainsKey(id))
            {
                operationResult = ModifyDataResultEnum.ErrNotFound;
                return null;
            }

            var item = resources[id];
            if (index < 0 || index + length > item.Length)
            {
                operationResult = ModifyDataResultEnum.ErrIndexOutside;
                return null;
            }

            var resultVal = item.Remove(index, length);
            resources[id] = resultVal;
            setLastChange(id, resultVal, nameof(UpdateRemoveSubstr));
            operationResult = ModifyDataResultEnum.Success;
            return resultVal;
        }

        public string UpdateReplaceSubstr(T id, string oldVal, string newVal, out ModifyDataResultEnum operationResult)
        {
            if (!resources.ContainsKey(id))
            {
                operationResult = ModifyDataResultEnum.ErrNotFound;
                return null;
            }

            var item = resources[id];

            var resultVal = item.Replace(oldVal, newVal);
            resources[id] = resultVal;

            setLastChange(id, resultVal, nameof(UpdateReplaceSubstr));
            operationResult = ModifyDataResultEnum.Success;
            return resultVal;
        }
    }
}
