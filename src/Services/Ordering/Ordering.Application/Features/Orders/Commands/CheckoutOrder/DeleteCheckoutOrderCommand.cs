using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class DeleteCheckoutOrderCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteCheckoutOrderCommandHandler : IRequestHandler<DeleteCheckoutOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCheckoutOrderCommandHandler> _logger;

        public DeleteCheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteCheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Unit> Handle(DeleteCheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await _orderRepository.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order {orderToDelete.Id} was deleted successful");

            return Unit.Value;
        }
    }
}
