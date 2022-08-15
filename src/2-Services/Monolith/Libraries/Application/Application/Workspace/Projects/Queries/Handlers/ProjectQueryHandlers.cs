﻿using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskoMask.Services.Monolith.Application.Workspace.Projects.Queries.Models;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Projects;
using TaskoMask.BuildingBlocks.Application.Queries;
using TaskoMask.BuildingBlocks.Contracts.Resources;
using TaskoMask.BuildingBlocks.Application.Exceptions;
using TaskoMask.BuildingBlocks.Application.Notifications;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.Services.Monolith.Domain.DataModel.Data;
using TaskoMask.BuildingBlocks.Domain.Resources;
using TaskoMask.BuildingBlocks.Contracts.Models;

namespace TaskoMask.Services.Monolith.Application.Workspace.Projects.Queries.Handlers
{
    public class ProjectQueryHandlers : BaseQueryHandler,
        IRequestHandler<GetProjectByIdQuery, ProjectOutputDto>,
        IRequestHandler<GetProjectsByOrganizationIdQuery, IEnumerable<ProjectBasicInfoDto>>,
        IRequestHandler<SearchProjectsQuery, PaginatedList<ProjectOutputDto>>,
        IRequestHandler<GetProjectsCountQuery, long>

    {
        #region Fields

        private readonly IProjectRepository _projectRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IBoardRepository _boardRepository;

        #endregion

        #region Ctors

        public ProjectQueryHandlers(IProjectRepository projectRepository, INotificationHandler notifications, IMapper mapper, IBoardRepository boardRepository, IOrganizationRepository organizationRepository) : base(mapper, notifications)
        {
            _projectRepository = projectRepository;
            _boardRepository = boardRepository;
            _organizationRepository = organizationRepository;
        }

        #endregion

        #region Handlers



        /// <summary>
        /// 
        /// </summary>
        public async Task<ProjectOutputDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null )
                throw new ApplicationException(ContractsMessages.Data_Not_exist, DomainMetadata.Project);

            var dto = _mapper.Map<ProjectOutputDto>(project);

            //TODO refactore read model for board to decrease db queries
            var organization = await _organizationRepository.GetByIdAsync(project.OrganizationId);
            dto.OrganizationName = organization.Name;
            return dto;
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<IEnumerable<ProjectBasicInfoDto>> Handle(GetProjectsByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetListByOrganizationIdAsync(request.OrganizationId);
            return _mapper.Map<IEnumerable<ProjectBasicInfoDto>>(projects);
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<PaginatedList<ProjectOutputDto>> Handle(SearchProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = _projectRepository.Search(request.Page, request.RecordsPerPage, request.Term, out var pageNumber, out var totalCount);
            var projectsDto = _mapper.Map<IEnumerable<ProjectOutputDto>>(projects);

            foreach (var item in projectsDto)
            {
                var organization = await _organizationRepository.GetByIdAsync(item.OrganizationId);
                item.OrganizationName = organization?.Name;
                item.BoardsCount = await _boardRepository.CountByProjectIdAsync(item.Id);
            }

            return new PaginatedList<ProjectOutputDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                Items = projectsDto
            };
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<long> Handle(GetProjectsCountQuery request, CancellationToken cancellationToken)
        {
            return await _projectRepository.CountAsync();
        }



        #endregion

    }
}
