using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Model;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class UpdateCheckoutOrderCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public string PhoneNumber { get; set; }
        public int PaymentMethod { get; set; }
    }

    public class UpdateCheckoutOrderCommandHandler : IRequestHandler<UpdateCheckoutOrderCommand, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<UpdateCheckoutOrderCommandHandler> _logger;

        public UpdateCheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<UpdateCheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(UpdateCheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            _mapper.Map(request, orderToUpdate, typeof(UpdateCheckoutOrderCommand), typeof(Order));

            var updatedOrder = await _orderRepository.UpdateAsync(orderToUpdate);

            _logger.LogInformation($"Order {updatedOrder.Id} was update successful created");

            await SendMail(updatedOrder);

            return updatedOrder.Id;
        }
        private async Task SendMail(Order order)
        {
            var email = new Email { Body = $"Order {order.Id} was updated successful", Subject = "Update Order", To = "antonicamwangi@gmail.com", ToName="Kamiri"};

            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Order {order.Id} failed due to error with the mail service in update :{ex.Message}");
            }
        }
    }

    public class UpdateCheckoutOrderCommandValidator : AbstractValidator<UpdateCheckoutOrderCommand>
    {
        public UpdateCheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
               .NotEmpty().WithMessage("{UserName} can not be empty")
               .NotNull()
               .MaximumLength(50).WithMessage("{UserName} can not exceed 50 characters");
            RuleFor(v => v.PhoneNumber)
                .NotEmpty()
                .NotNull().WithMessage("{PhoneNumber} is required.")
                .MinimumLength(10).WithMessage("{PhoneNumber} must not be less than 10 characters.")
                .MaximumLength(13).WithMessage("{PhoneNumber} must not exceed 13 characters.")
                .Matches(new Regex(@"(^(?:[0-9]\-?){5,13}[0-9]$)|(^(?:[0-9]\x20?){5,13}[0-9]$)")).WithMessage("{PhoneNumber} not valid");
            RuleFor(v => v.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required")
                .NotNull()
                .EmailAddress().WithMessage("A valid {EmailAddress} is required");
            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} can not be empty")
                .NotNull()
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero");
        }
    }
}
