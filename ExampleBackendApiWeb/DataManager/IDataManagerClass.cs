using ExampleBackendApiWeb.Enums;

namespace ExampleBackendApiWeb.DataManager
{
/// <summary>
/// класс-декоратор для настройки ResourceRepository и специфического расширения его функционала
/// </summary>
    public interface IDataManagerClass
    {
        /// <summary>
        /// устанавливается значение, если заблокировали общий объект данных. Если 0 - объект не блокировался.
        /// </summary>
        long blockStamp { get; set; }

        /// <summary>
        /// хранилище ресурсов
        /// </summary>
        IResourceRepository<int> resourceRepository { get; }

        /// <summary>
        /// Создать новый ресурс c id типа int
        /// </summary>
        /// <param name="value">значение</param>
        /// <param name="operationResult">ответ успех или ошибка</param>
        /// <returns>ID нового ресурса</returns>
        int CreateWithNewId(string value, out ModifyDataResultEnum operationResult);
    }
}