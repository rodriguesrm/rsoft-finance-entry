﻿using MediatR;
using RSoft.Entry.Contracts.Models;
using RSoft.Lib.Design.Application.Commands;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RSoft.Entry.Contracts.Commands
{

    /// <summary>
    /// Update Entry command contract 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ListEntryCommand : IRequest<CommandResult<IEnumerable<EntryDto>>>
    {

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<IEnumerable<EntryDto>> Response { get; set; }

        #endregion

    }
}
