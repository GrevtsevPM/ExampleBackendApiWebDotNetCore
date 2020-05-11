using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Config
{
    public static class MainConfig
    {
        /// <summary>
        /// имя объекта в кэше, указывающего, что в данный момент объект данных используется
        /// </summary>
        public const string dataResourceLockedKey = "dataResourceLocked";
        /// <summary>
        /// имя объекта в кэше, содержащего объект данных
        /// </summary>
        public const string dataResourceKey = "dataResource";
        /// <summary>
        /// период проверки занятости объекта данных в кэше
        /// </summary>
        public const int retryReadResourceDelay = 200;
        /// <summary>
        /// максимальное количество попыток получить ресурс
        /// </summary>
        public const int maxWaitForResourcePeriodsCount = 50;

        //webhook
        /// <summary>
        /// максимальное количество попыток отправки webhook
        /// </summary>
        public const int webhookMaxTrySendCount = 2;
        /// <summary>
        /// задержка перед повторной отправкой
        /// </summary>
        public const int webhookSendRetryDelay = 2000;
    }
}
