using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Queries.DTos;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVM>().ReverseMap(); 
            CreateMap<Order, CreateCheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateCheckoutOrderCommand>().ReverseMap();
        }

    }
}
