﻿using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskoMask.Services.Monolith.Application.Workspace.Tasks.Queries.Models;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Tasks;
using TaskoMask.BuildingBlocks.Application.Queries;
using TaskoMask.BuildingBlocks.Contracts.Resources;
using TaskoMask.BuildingBlocks.Application.Exceptions;
using TaskoMask.BuildingBlocks.Application.Notifications;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.Services.Monolith.Domain.DataModel.Data;
using TaskoMask.BuildingBlocks.Domain.Resources;
using TaskoMask.BuildingBlocks.Contracts.Models;

namespace TaskoMask.Services.Monolith.Application.Workspace.Tasks.Queries.Handlers
{
    public class TaskQueryHandlers : BaseQueryHandler,
        IRequestHandler<GetTaskByIdQuery, TaskBasicInfoDto>,
        IRequestHandler<GetTasksByCardIdQuery, IEnumerable<TaskBasicInfoDto>>,
        IRequestHandler<GetPendingTasksByBoardsIdQuery, IEnumerable<TaskBasicInfoDto>>,
        IRequestHandler<SearchTasksQuery, PaginatedList<TaskOutputDto>>,
        IRequestHandler<GetTasksCountQuery, long>

    {

        #region Fields

        private readonly ITaskRepository _taskRepository;
        private readonly ICardRepository _cardRepository;


        #endregion

        #region Ctors

        public TaskQueryHandlers(ITaskRepository taskRepository, INotificationHandler notifications, IMapper mapper, ICardRepository cardRepository) : base(mapper, notifications)
        {
            _taskRepository = taskRepository;
            _cardRepository = cardRepository;
        }

        #endregion

        #region Handlers




        /// <summary>
        /// 
        /// </summary>
        public async Task<TaskBasicInfoDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
                throw new ApplicationException(ContractsMessages.Data_Not_exist, DomainMetadata.Task);

            return _mapper.Map<TaskBasicInfoDto>(task);
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<IEnumerable<TaskBasicInfoDto>> Handle(GetTasksByCardIdQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetListByCardIdAsync(request.CardId);
            return _mapper.Map<IEnumerable<TaskBasicInfoDto>>(tasks);
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<IEnumerable<TaskBasicInfoDto>> Handle(GetPendingTasksByBoardsIdQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetPendingTasksByBoardsIdAsync(request.BoardsId, request.TakeCount);
            return _mapper.Map<IEnumerable<TaskBasicInfoDto>>(tasks);
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<PaginatedList<TaskOutputDto>> Handle(SearchTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = _taskRepository.Search(request.Page, request.RecordsPerPage, request.Term, out var pageNumber, out var totalCount);
            var tasksDto = _mapper.Map<IEnumerable<TaskOutputDto>>(tasks);

            foreach (var item in tasksDto)
            {
                var card = await _cardRepository.GetByIdAsync(item.CardId);
                item.CardName = card?.Name;
            }

            return new PaginatedList<TaskOutputDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                Items = tasksDto
            };
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task<long> Handle(GetTasksCountQuery request, CancellationToken cancellationToken)
        {
            return await _taskRepository.CountAsync();
        }



        #endregion
    }
}