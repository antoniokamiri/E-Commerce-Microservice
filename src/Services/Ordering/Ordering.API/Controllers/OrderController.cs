using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Queries.DTos;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{userName}", Name = "GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVM>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVM>>> GetOrdersByUsername(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);

            return Ok(orders);
        }

        [HttpPost(Name = "CheckOutOrder")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CreateCheckoutOrder([FromBody] CreateCheckoutOrderCommand command)
        {
            var order = await _mediator.Send(command);
            return Ok(order);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> UpdateCheckoutOrder([FromBody] UpdateCheckoutOrderCommand command)
        {
            var order = await _mediator.Send(command);
            return Ok(order);
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> DeleteCheckoutOrder(string id)
        {
            var command = new DeleteCheckoutOrderCommand() {Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
