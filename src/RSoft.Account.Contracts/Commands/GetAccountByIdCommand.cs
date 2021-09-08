﻿using MediatR;
using RSoft.Account.Contracts.Models;
using RSoft.Finance.Contracts.Commands;
using System;

namespace RSoft.Account.Contracts.Commands
{

    /// <summary>
    /// Update Account command contract 
    /// </summary>
    public class GetAccountByIdCommand : IRequest<CommandResult<AccountDto>>
    {

        #region Constructors

        /// <summary>
        /// Get Account by id
        /// </summary>
        /// <param name="id">Account id</param>
        public GetAccountByIdCommand(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Request Data

        /// <summary>
        /// Account id
        /// </summary>
        public Guid Id { get; set; }

        #endregion

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<AccountDto> Response { get; set; }

        #endregion

    }
}
