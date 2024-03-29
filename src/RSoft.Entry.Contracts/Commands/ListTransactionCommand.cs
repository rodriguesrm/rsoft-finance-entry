﻿using MediatR;
using RSoft.Entry.Contracts.FilterArguments;
using RSoft.Entry.Contracts.Models;
using RSoft.Finance.Contracts.Enum;
using RSoft.Lib.Design.Application.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RSoft.Entry.Contracts.Commands
{

    /// <summary>
    /// Update Transaction command contract 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ListTransactionCommand : IRequest<CommandResult<IEnumerable<TransactionDto>>>
    {

        #region Constructors

        /// <summary>
        /// Create a new command object
        /// </summary>
        /// <param name="periodDate">Date data to filter</param>
        /// <param name="periodYearMonth">Year/month to filter</param>
        /// <param name="entryId">Entry id to filter</param>
        /// <param name="transactionType">Transaction type to filter</param>
        /// <param name="paymentMethodId">Period method id to filter</param>
        public ListTransactionCommand
        (
            PeriodDateFilter periodDate = null,
            PeriodYearMonthFilter periodYearMonth = null, 
            Guid? entryId = null, 
            TransactionTypeEnum? transactionType = null, 
            Guid? paymentMethodId = null
        )
        {
            PeriodDate = periodDate;
            PeriodYearMonth = periodYearMonth;
            EntryId = entryId;
            TransactionType = transactionType;
            PaymentMethodId = paymentMethodId;
        }

        #endregion

        #region Request Data

        /// <summary>
        /// Filter by date range
        /// </summary>
        public PeriodDateFilter PeriodDate { get; set; }

        /// <summary>
        /// Filter by Year/Month
        /// </summary>
        public PeriodYearMonthFilter PeriodYearMonth { get; set; }

        /// <summary>
        /// Filter by Entry id
        /// </summary>
        public Guid? EntryId { get; set; }

        /// <summary>
        /// Filter by transaction type
        /// </summary>
        public TransactionTypeEnum? TransactionType { get; set; }

        /// <summary>
        /// Filter by payment method id
        /// </summary>
        public Guid? PaymentMethodId { get; set; }

        #endregion

        #region Result Data

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<IEnumerable<TransactionDto>> Response { get; set; }

        #endregion

    }
}
