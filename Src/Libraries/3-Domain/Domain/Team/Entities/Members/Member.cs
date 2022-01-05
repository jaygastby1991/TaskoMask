﻿using TaskoMask.Domain.Core.Models;
using TaskoMask.Domain.Core.Services;
using TaskoMask.Domain.Core.ValueObjects;
using TaskoMask.Domain.Team.Members.Events;

namespace TaskoMask.Domain.Team.Entities.Members
{
    /// <summary>
    /// Members are those who manage their tasks in this system
    /// </summary>
    public class Member : BaseAggregate
    {
        #region Fields


        #endregion

        #region Ctors

        public Member(UserIdentity identity, UserAuthentication authentication, IEncryptionService encryptionService)
        {
          //  AddDomainEvent(new MemberCreatedEvent(Id, displayName.Value, email.Value, ));
        }



        #endregion

        #region Properties

        public UserIdentity Identity { get; private set; }
        public UserAuthentication Authentication { get; private set; }

        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public  void Update(string displayName, string email, string userName)
        {
            base.Update();

           // AddDomainEvent(new MemberUpdatedEvent(Id, DisplayName, Email, UserName));
        }





        #endregion

        #region Private Methods



        /// <summary>
        /// 
        /// </summary>
        protected override void CheckInvariants()
        {

        }



        #endregion
    }
}