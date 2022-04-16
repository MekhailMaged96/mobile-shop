using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMapper mapper, IMediator mediator,ILogger<BasketCheckoutConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            try
            {
                var checkoutOrderCommand = _mapper.Map<CheckoutOrderCommand>(context.Message);

                var result = await _mediator.Send(checkoutOrderCommand);

                _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Faild To Add Order ", ex.Message);
            }
 
        }
    }
}
