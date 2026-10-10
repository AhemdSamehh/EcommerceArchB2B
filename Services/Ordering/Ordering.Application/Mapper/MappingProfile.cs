using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Ordering.Commands.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap<BasketCheckoutEvent, CreateOrderDto>().ReverseMap();
        }
        public void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                      .Where(t => t.GetInterfaces().Any(i =>
                          i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                      .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                    ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }

        }
    }
}
