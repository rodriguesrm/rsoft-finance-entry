﻿using MediatR;
using RSoft.Finance.Contracts.Commands;
using System;

namespace RSoft.Account.Contracts.Commands
{

    /// <summary>
    /// Create PaymentMethod command contract 
    /// </summary>
    public class ChangeStatusPaymentMethodCommand : IRequest<CommandResult<bool>>
    {

        #region Constructors

        /// <summary>
        /// Create a new command instance
        /// </summary>
        /// <param name="id">PaymentMethod id</param>
        /// <param name="isActive">Active status flag</param>
        public ChangeStatusPaymentMethodCommand(Guid id, bool isActive)
        {
            Id = id;
            IsActive = isActive;
        }

        #endregion

        #region Request Data


        /// <summary>
        /// PaymentMethod id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Active status flag
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<bool> Response { get; set; }

        #endregion

    }
}