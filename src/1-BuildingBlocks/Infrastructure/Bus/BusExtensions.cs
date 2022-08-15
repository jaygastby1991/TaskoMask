﻿using Microsoft.Extensions.DependencyInjection;

namespace TaskoMask.BuildingBlocks.Infrastructure.Bus
{
    public static class BusExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddInMemoryBus(this IServiceCollection services)
        {
            return services.AddScoped<IInMemoryBus, InMemoryBus>();
        }
    }
}
