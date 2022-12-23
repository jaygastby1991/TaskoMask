﻿using AutoMapper;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Boards;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Common;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Organizations;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Projects;
using TaskoMask.BuildingBlocks.Contracts.Protos;

namespace TaskoMask.ApiGateways.UserPanel.Aggregator.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreationTime,  CreationTimeDto> ()
                 .ForMember(dest => dest.CreateDateTime, opt =>
                      opt.MapFrom(src => src.CreateDateTime.ToDateTime()))
                .ForMember(dest => dest.ModifiedDateTime, opt =>
                      opt.MapFrom(src => src.ModifiedDateTime.ToDateTime()));

            CreateMap<OrganizationBasicInfoGrpcResponse, OrganizationBasicInfoDto>();

            CreateMap<ProjectBasicInfoGrpcResponse, ProjectBasicInfoDto>();

            CreateMap<GetBoardsByProjectIdGrpcResponse, GetBoardDto>();

            CreateMap<GetBoardByIdGrpcResponse, GetBoardDto>();
        }
    }
}
