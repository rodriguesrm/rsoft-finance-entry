﻿using MediatR;
using RSoft.Entry.Contracts.Models;
using RSoft.Lib.Design.Application.Commands;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RSoft.Entry.Contracts.Commands
{

    /// <summary>
    /// Update PaymentMethod command contract 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ListPaymentMethodCommand : IRequest<CommandResult<IEnumerable<PaymentMethodDto>>>
    {

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<IEnumerable<PaymentMethodDto>> Response { get; set; }

        #endregion

    }
}
