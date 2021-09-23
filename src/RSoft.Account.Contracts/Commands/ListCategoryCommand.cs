﻿using MediatR;
using RSoft.Account.Contracts.Models;
using RSoft.Lib.Design.Application.Commands;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RSoft.Account.Contracts.Commands
{

    /// <summary>
    /// Update category command contract 
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Anemic class")]
    public class ListCategoryCommand : IRequest<CommandResult<IEnumerable<CategoryDto>>>
    {

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<IEnumerable<CategoryDto>> Response { get; set; }

        #endregion

    }
}
