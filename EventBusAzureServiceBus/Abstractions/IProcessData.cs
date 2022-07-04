using EAuction.Order.Application.Commands.OrderCreate;
using EAuction.Order.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusAzureServiceBus.Abstractions
{
    public interface IProcessData
    {
        Task<BidResponse> Process(BidCreateCommand myPayload);
    }
}
