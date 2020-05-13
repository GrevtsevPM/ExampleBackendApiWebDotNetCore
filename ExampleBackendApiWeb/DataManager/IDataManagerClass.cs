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
        IResourceRepositoryIntId resourceRepository { get; }
    }
}