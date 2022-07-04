using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EAuction.Order.Application.Commands.OrderCreate;
using EventBusAzureServiceBus.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBusAzureServiceBus
{
    public class TopicSubscription : ITopicSubscription
    {
        private readonly IProcessData _processAddressData;
        private readonly IConfiguration _configuration;
        private const string TOPIC_PATH = "bids";
        private const string SUBSCRIPTION_NAME = "Bids";
        private readonly ServiceBusClient _client;
        private readonly ServiceBusAdministrationClient _adminClient;
        private ServiceBusProcessor _processor;

        public TopicSubscription(IProcessData processData,
            IConfiguration configuration)
        {
            _processAddressData = processData;
            _configuration = configuration;

            //var connectionString = _configuration.GetConnectionString("ServiceBusConnectionString");
            var connectionString = "Endpoint=sb://sb-hackfse-eauction.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=p82vrUwqVqbFi5fk+XdGpxMUpVTN/5n3EQsV+jAAvRM=";
            _client = new ServiceBusClient(connectionString);
            _adminClient = new ServiceBusAdministrationClient(connectionString);
        }

        public async Task StartHandlingMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(TOPIC_PATH, SUBSCRIPTION_NAME, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }
        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<BidCreateCommand>();
            await _processAddressData.Process(myPayload).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.Error.WriteLine(arg.Exception);
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseSubscriptionAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
