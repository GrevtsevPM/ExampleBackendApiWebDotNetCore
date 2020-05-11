using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExampleBackendApiWeb.ApiParameters.ResourcesManagerController;
using ExampleBackendApiWeb.Config;
using ExampleBackendApiWeb.DataManager;
using ExampleBackendApiWeb.DataManager.Models;
using ExampleBackendApiWeb.Enums;
using ExampleBackendApiWeb.Results;
using ExampleBackendApiWeb.Webhook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ExampleBackendApiWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesManagerController : ControllerBase
    {
        private IMemoryCache _cache;

        private readonly IHttpClientFactory _clientFactory;

        public ResourcesManagerController(IHttpClientFactory clientFactory, Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// получение объекта данных из памяти или инициализация, если его нет
        /// </summary>
        /// <param name="blockAccess">флаг требуется ли блокировка доступа</param>
        /// <returns>декоратор объекта с данными </returns>
        async Task<IDataManagerClass> getDataManager(bool blockAccess = false)
        {
            object dataResourceLockObj;
            int waitPeriodsCount = 0;
            do
            {
                //получаем значение блокировки ресурса
                dataResourceLockObj = _cache.Get(MainConfig.dataResourceLockedKey);
                //если не заблокирован
                if (dataResourceLockObj == null)
                {
                    long blockStamp = 0;
                    //блокировка доступа
                    if (blockAccess)
                    {
                        blockStamp = DateTime.Now.Ticks;
                        //запись объекта блокировки в память
                        _cache.Set(MainConfig.dataResourceLockedKey, blockStamp);
                    }

                    try
                    {
                        //получаем объект данных из памяти
                        var dataResourceObj = _cache.Get(MainConfig.dataResourceKey);
                        IDataManagerClass dataManager;
                        //если объекта данных нет в памяти, инициализируем и сохраняем
                        if (dataResourceObj == null)
                        {
                            dataManager = new DataManagerClass();
                            _cache.Set(MainConfig.dataResourceKey, dataManager.resourceRepository.Resources);
                        }
                        else
                        {
                            dataManager = new DataManagerClass((Dictionary<int, string>)dataResourceObj);
                        }
                        dataManager.blockStamp = blockStamp;
                        return dataManager;
                    }
                    catch (Exception ex)
                    {
                        if(blockAccess) unlockDataObject();
                        throw ex;
                    }
                }
                //если объект используется, пауза
                else
                {
                    await Task.Delay(MainConfig.retryReadResourceDelay);
                }

            } while (dataResourceLockObj != null && waitPeriodsCount++ < MainConfig.maxWaitForResourcePeriodsCount);

            throw new Exception($"getDataManager: null; {nameof(waitPeriodsCount)}: {waitPeriodsCount}");
        }

        /// <summary>
        /// сохранение общего объекта данных (объект уже заблокирован нами, поэтому метод синхронный)
        /// </summary>
        void updateDataObject(IDataManagerClass dataManager)
        {
            try
            {
                object dataResourceLockObj = _cache.Get(MainConfig.dataResourceLockedKey);
                //проверка заблокирован ли объект. Должен быть заблокирован. Если нет - exception.
                if (dataResourceLockObj == null)
                {
                    throw new Exception("При попытке сохранить объект данных, отсутствует в памяти запись о блокировке");
                }
                //проверка что объект значение отметки блокировки идентично нашему и не было переписано другим кодом (дополнительная проверка)
                if ((long)dataResourceLockObj != dataManager.blockStamp)
                {
                    throw new Exception("При попытке сохранить объект данных, blockStamp не соответствует имеющемуся");
                }

                //сохраняем объект данных в память
                _cache.Set(MainConfig.dataResourceKey, dataManager.resourceRepository.Resources);
            }
            finally
            {
                unlockDataObject();
            }

            if (dataManager.resourceRepository.SendWebhook)
            {
                sendWebhook(new Webhook1Item(dataManager.resourceRepository.LastChangedId, dataManager.resourceRepository.LastChangedValue, dataManager.resourceRepository.LastChangedEventType));
            }
        }

        /// <summary>
        /// удалить блокировку объекта данных
        /// </summary>
        void unlockDataObject()
        {
            _cache.Remove(MainConfig.dataResourceLockedKey);
        }


        /// <summary>
        /// отправка Webhook
        /// </summary>
        /// <param name="item">отправляемые данные</param>
        void sendWebhook(Webhook.Webhook1Item item)
        {
            var pts = new ParameterizedThreadStart(Webhook1.Send);
            var client = _clientFactory.CreateClient("Webhook1");

            var thread = new Thread(pts);
            //запускаем отправку Webhook в новом потоке
            thread.Start(new Webhook1StartParam(client, item));
        }


        /// <summary>
        /// Получить список всех ресурсов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResourceItemModel<int>>> Get()
        {
            var dataManager = await getDataManager();
            return dataManager.resourceRepository.GetAll().ToList();
        }

        /// <summary>
        /// Получить ресурс по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Значение ресурса. Если не найден по ID - статус 400</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dataManager = await getDataManager();
            ModifyDataResultEnum operationResult;
            var res = dataManager.resourceRepository.GetById(id, out operationResult);

            if (ModifyDataResultHelper.IsResultError(operationResult)) 
                return BadRequest(new ModifyDataBadRequestResult(operationResult));
            return Ok(res);
        }

        /// <summary>
        /// Получить подстроку ресурса по индексу начала и длине подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start">индекс начала</param>
        /// <param name="length">длина подстроки</param>
        /// <returns>подстрока</returns>
        [HttpGet("Substring/{id}")]
        public async Task<IActionResult> Substring(int id, int start, int length)
        {
            var dataManager = await getDataManager();
            ModifyDataResultEnum operationResult;
            var res = dataManager.resourceRepository.GetSubstring(id, start, length, out operationResult);
            
            if (ModifyDataResultHelper.IsResultError(operationResult)) 
                return BadRequest(new ModifyDataBadRequestResult(operationResult));
            return Ok(res);
        }

        /// <summary>
        /// Создать новый ресурс
        /// </summary>
        /// <param name="value">значение</param>
        /// <returns>ID нового ресурса</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]string value)
        {
            var dataManager = await getDataManager(true);
            try
            {
                ModifyDataResultEnum operationResult;
                var newId = dataManager.CreateWithNewId(value, out operationResult);

                if (ModifyDataResultHelper.IsResultError(operationResult))
                {
                    unlockDataObject();
                    return BadRequest(new ModifyDataBadRequestResult(operationResult));
                }
                updateDataObject(dataManager);
                return Ok(newId);
            }
            catch (Exception ex)
            {
                unlockDataObject();
                throw ex;
            }
}

        /// <summary>
        /// Перезаписать ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">значение</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm]string value)
        {
            var dataManager = await getDataManager(true);
            try
            {
                ModifyDataResultEnum operationResult;
                operationResult = dataManager.resourceRepository.ReplaceItem(id, value);

                if (ModifyDataResultHelper.IsResultError(operationResult))
                {
                    unlockDataObject();
                    return BadRequest(new ModifyDataBadRequestResult(operationResult));
                }
                updateDataObject(dataManager);
                return Ok();
            }
            catch (Exception ex)
            {
                unlockDataObject();
                throw ex;
            }
        }

        /// <summary>
        /// Удалить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dataManager = await getDataManager(true);
            try
            {
                ModifyDataResultEnum operationResult;
                operationResult = dataManager.resourceRepository.Delete(id);

                if (ModifyDataResultHelper.IsResultError(operationResult))
                {
                    unlockDataObject();
                    return BadRequest(new ModifyDataBadRequestResult(operationResult));
                }
                updateDataObject(dataManager);
                return Ok();
            }
            catch (Exception ex)
            {
                unlockDataObject();
                throw ex;
            }
        }

        /// <summary>
        /// Частичное обновление ресурса по ID: Вставка подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">подстрока</param>
        /// <param name="updateType">тип вставки</param>
        /// <param name="index">индекс (для вставки по индексу)</param>
        /// <returns></returns>
        [HttpPatch("InsertSubstr/{id}")]
        public async Task<IActionResult> InsertSubstr(int id, [FromForm]UpdateInsertSubstrParameter p)
        {
            var dataManager = await getDataManager(true);
            try
            {
                ModifyDataResultEnum operationResult;
                var resultVal = dataManager.resourceRepository.UpdateInsertSubstr(id, p.value, p.updateType, out operationResult, p.index);

                if (ModifyDataResultHelper.IsResultError(operationResult))
                {
                    unlockDataObject();
                    return BadRequest(new ModifyDataBadRequestResult(operationResult));
                }
                updateDataObject(dataManager);
                return Ok(resultVal);
            }
            catch (Exception ex)
            {
                unlockDataObject();
                throw ex;
            }
        }

        /// <summary>
        /// Частичное обновление ресурса по ID: Удаление подстроки по индексу и длине
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index">индекс</param>
        /// <param name="length">длина</param>
        /// <returns></returns>
        [HttpPatch("RemoveSubstr/{id}")]
        public async Task<IActionResult> RemoveSubstr(int id, [FromForm]int index, [FromForm]int length)
        {
            var dataManager = await getDataManager(true);
            try
            {
                ModifyDataResultEnum operationResult;
                var resultVal = dataManager.resourceRepository.UpdateRemoveSubstr(id, index, length, out operationResult);

                if (ModifyDataResultHelper.IsResultError(operationResult))
                {
                    unlockDataObject();
                    return BadRequest(new ModifyDataBadRequestResult(operationResult));
                }
                updateDataObject(dataManager);
                return Ok(resultVal);
            }
            catch (Exception ex)
            {
                unlockDataObject();
                throw ex;
            }
        }

        /// <summary>
        /// Частичное обновление ресурса по ID: Замена подстроки на другую
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldVal">подстрока которую заменить</param>
        /// <param name="newVal">на что заменить</param>
        /// <returns></returns>
        [HttpPatch("ReplaceSubstr/{id}")]
        public async Task<IActionResult> ReplaceSubstr(int id, [FromForm]string oldVal, [FromForm]string newVal)
        {
            var dataManager = await getDataManager(true);
            try
            {
                ModifyDataResultEnum operationResult;
                var resultVal = dataManager.resourceRepository.UpdateReplaceSubstr(id, oldVal, newVal, out operationResult);

                if (ModifyDataResultHelper.IsResultError(operationResult))
                {
                    unlockDataObject();
                    return BadRequest(new ModifyDataBadRequestResult(operationResult));
                }
                updateDataObject(dataManager);
                return Ok(resultVal);
            }
            catch (Exception ex)
            {
                unlockDataObject();
                throw ex;
            }
        }

        /// <summary>
        /// ДЛЯ ТЕСТИРОВАНИЯ список полученных Webhook
        /// </summary>
        /// <returns></returns>
        [HttpGet("Webhook1TestList")]
        public IActionResult Webhook1TestList()
        {
            var webhookList = _cache.Get("webhookList");
            if (webhookList != null) return Ok(webhookList);
            return Ok();
        }

        /// <summary>
        /// ДЛЯ ТЕСТИРОВАНИЯ сохранение Webhook
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost("Webhook1TestSave")]
        public IActionResult Webhook1TestSave([FromBody]string message)
        {
            var webhookListObj = _cache.Get("webhookList");
            var list = webhookListObj as List<string> ?? new List<string>();
            list.Add(message);
            _cache.Set("webhookList", list);

            return Ok();
        }
    }
}
