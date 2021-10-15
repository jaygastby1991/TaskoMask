﻿using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskoMask.Application.Common.BaseUsers.Commands.Models;
using TaskoMask.Application.Core.Bus;
using TaskoMask.Application.Core.Commands;
using TaskoMask.Application.Core.Exceptions;
using TaskoMask.Application.Core.Notifications;
using TaskoMask.Application.Core.Resources;
using TaskoMask.Domain.Core.Data;
using TaskoMask.Domain.Core.Models;
using TaskoMask.Domain.Core.Resources;

namespace Aghoosh.Application.Common.BaseUsers.Commands.Handlers
{
    public class UserCommandHandlers<TEntity> : BaseCommandHandler,
        IRequestHandler<SetUserIsActiveCommand<TEntity>, CommandResult> where TEntity : BaseUser

    {
        #region Fields


        private readonly IUserBaseRepository<TEntity> _userRepository;
        private readonly IMapper _mapper;


        #endregion


        #region Ctor


        public UserCommandHandlers(IUserBaseRepository<TEntity> userRepository, IMapper mapper, IDomainNotificationHandler notifications, IInMemoryBus _inMemoryBu) : base(notifications, _inMemoryBu)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        #endregion


        #region Handle Methods


        /// <summary>
        /// 
        /// </summary>
        public async Task<CommandResult> Handle(SetUserIsActiveCommand<TEntity> request, CancellationToken cancellationToken)
        {
      
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new ApplicationException(ApplicationMessages.Data_Not_exist, DomainMetadata.User);

            user.SetActive(request.IsActive);
            if (!IsValid(user))
                return new CommandResult(ApplicationMessages.Update_Failed);

            await _userRepository.UpdateAsync(user);

            return new CommandResult(ApplicationMessages.Update_Success, request.Id);
        }


     

        #endregion

        
    }
}