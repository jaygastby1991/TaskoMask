﻿using Microsoft.Extensions.DependencyInjection;
using TaskoMask.BuildingBlocks.Infrastructure.Bus;
using TaskoMask.BuildingBlocks.Infrastructure.EventSourcing;
using TaskoMask.BuildingBlocks.Infrastructure.MongoDB;

namespace TaskoMask.BuildingBlocks.Infrastructure
{
    public static class InfrastructureExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddBuildingBlocksInfrastructureServices(this IServiceCollection services)
        {
            
            services.AddMongoDBBaseRepository();
            services.AddInMemoryBus();
            services.AddRedisEventStoreService();

            return services;
        }
    }
}
