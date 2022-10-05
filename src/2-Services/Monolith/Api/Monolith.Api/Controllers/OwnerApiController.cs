﻿using Microsoft.AspNetCore.Mvc;
using TaskoMask.Services.Monolith.Application.Workspace.Owners.Services;
using Microsoft.AspNetCore.Authorization;
using TaskoMask.BuildingBlocks.Web.MVC.Controllers;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.BuildingBlocks.Web.ApiContracts;
using TaskoMask.BuildingBlocks.Contracts.Dtos.Workspace.Owners;
using TaskoMask.BuildingBlocks.Contracts.Services;
using TaskoMask.BuildingBlocks.Contracts.Models;

namespace TaskoMask.Services.Monolith.Api.Controllers
{
    [Authorize("full-access")]
    public class OwnerApiController : BaseApiController, IOwnerApiService
    {
        #region Fields

        private readonly IOwnerService _ownerService;

        #endregion

        #region Ctors

        public OwnerApiController(IOwnerService ownerService, IAuthenticatedUserService authenticatedUserService) : base(authenticatedUserService)
        {
            _ownerService = ownerService;
        }

        #endregion

        #region Public Methods



        /// <summary>
        /// register new owner
        /// </summary>
        [HttpPost]
        [Route("owner")]
        [AllowAnonymous]
        public async Task<Result<CommandResult>> Register(RegisterOwnerDto input)
        {
            return  await _ownerService.RegisterAndSeedDefaultWorkspaceAsync(input);
        }






        /// <summary>
        /// get current owner basic information
        /// </summary>
        [HttpGet]
        [Route("owner")]
        public async Task<Result<OwnerBasicInfoDto>> Get()
        {
            return await _ownerService.GetByIdAsync(GetCurrentUserId());
        }




        /// <summary>
        /// Update current owner
        /// </summary>
        [HttpPut]
        [Route("owner")]
        public async Task<Result<CommandResult>> UpdateProfile([FromBody] UpdateOwnerProfileDto input)
        {
            input.Id = GetCurrentUserId();
            return await _ownerService.UpdateProfileAsync(input);
        }



        #endregion


        #region Private Methods



        #endregion

    }
}
