﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TaskoMask.BuildingBlocks.Domain.Data;
using TaskoMask.Services.Monolith.Domain.DataModel.Entities;

namespace TaskoMask.Services.Monolith.Domain.DataModel.Data
{

    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetListByTaskIdAsync(string taskId);
    }
}