using AutoMapper;
using EAuction.Order.Application.Commands.OrderCreate;
using EAuction.Order.Application.Responses;
using EventBusAzureServiceBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAuction.Order.WebApi.Consumers
{
    public class ProcessData: IProcessData
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ProcessData(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<BidResponse> Process(BidCreateCommand myPayload)
        {
            var command = _mapper.Map<BidCreateCommand>(myPayload);
            command.CreatedAt = DateTime.Now;
            command.BidAmount = myPayload.BidAmount;

            var result = await _mediator.Send(command);
            return result;
        }
    }
}
