﻿using MediatR;
using TaskoMask.Application.Core.Dtos.Tasks;
using TaskoMask.Application.Core.Queries;

namespace TaskoMask.Application.TaskManagement.Tasks.Queries.Models
{
   
    public class GetTaskByIdQuery : BaseQuery<TaskBasicInfoDto>
    {
        public GetTaskByIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}