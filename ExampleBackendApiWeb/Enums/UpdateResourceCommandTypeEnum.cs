using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Enums
{
    /// <summary>
    /// как обновить ресурс
    /// </summary>
    public enum UpdateResourceCommandTypeEnum
    {
        Undef = 0,
        /// <summary>
        /// Вставка подстроки в начало
        /// </summary>
        InsertAtStart = 1,
        /// <summary>
        /// Вставка подстроки в конец
        /// </summary>
        InsertAtEnd = 2,
        /// <summary>
        /// Вставка подстроки в любое место по индексу
        /// </summary>
        InsertAtIndex = 3
    }
}
