using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBusAzureServiceBus.Abstractions
{
    public interface ITopicSubscription
    {
        Task StartHandlingMessages();
        Task CloseSubscriptionAsync();
        ValueTask DisposeAsync();
    }
}
