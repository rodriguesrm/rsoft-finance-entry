﻿using RSoft.Entry.Infra.Tables;
using RSoft.Lib.Common.ValueObjects;
using System;
using TransactionDomain = RSoft.Entry.Core.Entities.Transaction;

namespace RSoft.Entry.Infra.Extensions
{

    /// <summary>
    /// Transaction extesions
    /// </summary>
    public static class TransactionExtension
    {

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        public static TransactionDomain Map(this Transaction table)
            => Map(table, true, false);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Load related data</param>
        /// <param name="forceLoadNameForLevelUp">Load name for entities in level up (as Category, Payment Method, etc)</param>
        public static TransactionDomain Map(this Transaction table, bool useLazy, bool forceLoadNameForLevelUp)
        {
            TransactionDomain result = null;

            if (table != null)
            {

                result = new TransactionDomain(table.Id)
                {
                    CreatedOn = table.CreatedOn,
                    Date = table.Date,
                    TransactionType = table.TransactionType,
                    Amount = table.Amount,
                    Comment = table.Comment
                };

                if (useLazy)
                {
                    result.PaymentMethod = table.PaymentMethod?.Map(false);
                    result.Entry = table.Entry?.Map(false);
                }
                else
                {
                    result.PaymentMethod = new Core.Entities.PaymentMethod(table.PaymentMethodId);
                    result.Entry = new Core.Entities.Entry(table.EntryId);
                }
                if (forceLoadNameForLevelUp)
                {
                    result.Entry.Name = table.Entry?.Name;
                    result.PaymentMethod.Name = table.PaymentMethod?.Name;
                }
                result.CreatedAuthor = new Author<Guid>(table.CreatedBy, (table.CreatedAuthor?.GetFullName() ?? "***"));

            }

            return result;

        }

        /// <summary>
        /// Maps entity to table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        public static Transaction Map(this TransactionDomain entity)
        {

            Transaction result = null;

            if (entity != null)
            {
                result = new Transaction(entity.Id)
                {
                    Year = entity.Year,
                    Month = entity.Month,
                    Date = entity.Date.Value,
                    TransactionType = entity.TransactionType,
                    Amount = entity.Amount,
                    Comment = entity.Comment,
                    EntryId = entity.Entry.Id,
                    PaymentMethodId = entity.PaymentMethod.Id,
                    CreatedOn = entity.CreatedOn,
                    CreatedBy = entity.CreatedAuthor.Id
                };
            }

            return result;

        }

        /// <summary>
        /// Maps entity to an existing table
        /// </summary>
        /// <param name="entity">Domain entity to map</param>
        /// <param name="table">Instance of existing table entity</param>
        public static Transaction Map(this TransactionDomain entity, Transaction table)
        {

            if (entity != null && table != null)
            {
                table.Date = entity.Date.Value;
                table.TransactionType = entity.TransactionType;
                table.Amount = entity.Amount;
                table.Comment = entity.Comment;
                table.EntryId = entity.Entry.Id;
                table.PaymentMethodId = entity.PaymentMethod.Id;
                table.CreatedOn = entity.CreatedOn;
                table.CreatedBy = entity.CreatedAuthor.Id;
            }

            return table;

        }

    }

}
