using ExampleBackendApiWeb.Enums;

namespace ExampleBackendApiWeb.DataManager
{
    public interface IResourceRepositoryIntId: IResourceRepository<int>
    {
        /// <summary>
        /// Создать новый ресурс c id типа int
        /// </summary>
        /// <param name="value">значение</param>
        /// <param name="operationResult">ответ успех или ошибка</param>
        /// <returns>ID нового ресурса</returns>
        int Create(out ModifyDataResultEnum operationResult, string value = "");
    }
}