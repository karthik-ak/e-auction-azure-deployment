using Azure.Messaging.ServiceBus;
using EventBusAzureServiceBus.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventBusAzureServiceBus
{
    public class TopicSender : ITopicSender
    {
        private const string TOPIC_PATH = "e-auction-queue";
        private readonly ServiceBusClient _client;
        private readonly Azure.Messaging.ServiceBus.ServiceBusSender _clientSender;

        public TopicSender(IConfiguration configuration)
        {
            var connectionString = "Endpoint=sb://sb-hackfse-eauction.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=p82vrUwqVqbFi5fk+XdGpxMUpVTN/5n3EQsV+jAAvRM=";
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(TOPIC_PATH);
        }

        public async Task SendMessage(object payload)
        {
            string messagePayload = JsonSerializer.Serialize(payload);
            ServiceBusMessage message = new ServiceBusMessage(messagePayload);
            message.ContentType = "application/json";
            try
            {
                await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
