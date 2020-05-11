using ExampleBackendApiWeb.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExampleBackendApiWeb.Webhook
{
    public class Webhook1
    {
        public Webhook1()
        {
            
        }

        /// <summary>
        /// Отправить Webhook1
        /// </summary>
        /// <param name="_param"></param>
        public static async void Send(object _param)
        {
            Webhook1StartParam param = (Webhook1StartParam)_param;
            int sendTryCount = 0;
            var webhookUrl = "ResourcesManager/Webhook1TestSave";
            do
            {
                var request = new HttpRequestMessage(HttpMethod.Post, webhookUrl);
                var content = $"\"ID: {param.item.LastChangedId}; Событие: {param.item.LastChangedEventType}; Новое значение: {param.item.LastChangedValue}\"";
                request.Content = new StringContent(
                    content, 
                    Encoding.UTF8
                    , "application/json");
                var response = await param.client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    await Task.Delay(MainConfig.webhookSendRetryDelay);
                }
            }
            while (sendTryCount++ < MainConfig.webhookMaxTrySendCount);

            //возможно выбрасывать исключение для лога
            //throw new Exception($"Webhook1 send failed, url: {webhookUrl}, tried count: {sendTryCount}");
        }
    }
}
