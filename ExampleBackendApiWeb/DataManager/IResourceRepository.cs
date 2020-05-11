using System.Collections.Generic;
using ExampleBackendApiWeb.DataManager.Models;
using ExampleBackendApiWeb.Enums;

namespace ExampleBackendApiWeb.DataManager
{
    /// <summary>
    /// класс для хранения и обработки данных "ресурсов". T - тип id
    /// </summary>
    public interface IResourceRepository<T>
    {
        string LastChangedEventType { get; }
        T LastChangedId { get; }
        string LastChangedValue { get; }
        /// <summary>
        /// получение копии объекта данных
        /// </summary>

        /// <summary>
        /// отправлять ли Webhook
        /// </summary>
        bool SendWebhook { get; set; }

        /// <summary>
        /// получение копии объекта данных
        /// </summary>
        Dictionary<T, string> Resources { get; }

        /// <summary>
        /// Создать ресурс (с возможностью предоставить начальное значение)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>ответ успех или ошибка</returns>
        ModifyDataResultEnum Create(T newId, string value = "");

        /// <summary>
        /// Удалить ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ответ успех или ошибка</returns>
        ModifyDataResultEnum Delete(T id);

        /// <summary>
        /// Получить список всех ресурсов
        /// </summary>
        /// <returns>список всех ресурсов</returns>
        /// 
        IEnumerable<ResourceItemModel<T>> GetAll();

        /// <summary>
        /// Получить ресурс по ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="operationResult">ответ успех или ошибка</param>
        /// <returns>ресурс</returns>
        string GetById(T id, out ModifyDataResultEnum operationResult);

        /// <summary>
        /// Получить подстроку ресурса по индексу начала и длине подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start">индекс начала</param>
        /// <param name="length">длина подстроки</param>
        /// <param name="operationResult">ответ успех или ошибка</param>
        /// <returns>подстрока</returns>
        string GetSubstring(T id, int start, int length, out ModifyDataResultEnum operationResult);

        /// <summary>
        /// Перезаписать ресурс
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">новое значение</param>
        /// <returns>ответ успех или ошибка</returns>
        ModifyDataResultEnum ReplaceItem(T id, string value);

        /// <summary>
        /// Частичное обновление ресурса по ID: Вставка подстроки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">подстрока</param>
        /// <param name="updateType">тип вставки</param>
        /// <param name="index">индекс (для вставки по индексу)</param>
        /// <returns>ответ успех или ошибка</returns>
        string UpdateInsertSubstr(T id, string value, UpdateResourceCommandTypeEnum updateType, out ModifyDataResultEnum operationResult, int index = -1);

        /// <summary>
        /// Частичное обновление ресурса по ID: Удаление подстроки по индексу и длине
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index">индекс</param>
        /// <param name="length">длина</param>
        /// <returns>ответ успех или ошибка</returns>
        string UpdateRemoveSubstr(T id, int index, int length, out ModifyDataResultEnum operationResult);

        /// <summary>
        /// Частичное обновление ресурса по ID: Замена подстроки на другую
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldVal">подстрока которую заменить</param>
        /// <param name="newVal">на что заменить</param>
        /// <returns>ответ успех или ошибка</returns>
        string UpdateReplaceSubstr(T id, string oldVal, string newVal, out ModifyDataResultEnum operationResult);
    }
}