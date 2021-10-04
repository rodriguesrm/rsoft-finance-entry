﻿using MediatR;
using RSoft.Lib.Design.Application.Commands;
using System.Diagnostics.CodeAnalysis;

namespace RSoft.Entry.Contracts.Commands
{

    /// <summary>
    /// Start Accrual Period command contract 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StartAccrualPeriodCommand : IRequest<CommandResult<bool>>
    {

        #region Constructors

        /// <summary>
        /// Create command instance
        /// </summary>
        /// <param name="year">year number</param>
        /// <param name="month">Month number</param>
        public StartAccrualPeriodCommand(int year, int month)
        {
            Year = year;
            Month = month;
        }

        #endregion

        #region Request Data

        /// <summary>
        /// Accrual period year
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// Accrual period month
        /// </summary>
        public int Month { get; private set; }

        #endregion

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<bool> Response { get; set; }

        #endregion

    }
}
