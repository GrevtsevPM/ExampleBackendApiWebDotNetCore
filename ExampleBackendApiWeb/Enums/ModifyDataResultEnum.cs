using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Enums
{
    public enum ModifyDataResultEnum
    {
        Undef = 0,
        /// <summary>
        /// Успешно
        /// </summary>
        Success = 1,
        /// <summary>
        /// Ошибка неопределенная
        /// </summary>
        Error = -1,
       
        /// <summary>
        /// переданы неверные параметры
        /// </summary>
        ErrWrongParams = -2,
        /// <summary>
        /// ресурс не найден по ID
        /// </summary>
        ErrNotFound = -10,
        /// <summary>
        /// значение не прошло валидацию
        /// </summary>
        ErrInvalidValue = -11,
        /// <summary>
        /// такой id уже используется
        /// </summary>
        ErrConflict = -12,
        /// <summary>
        /// строка null
        /// </summary>
        ErrStringNull = -13,
        /// <summary>
        /// индекс не в пределах допустимого
        /// </summary>
        ErrIndexOutside = -14

    }
}
