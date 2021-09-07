﻿using RSoft.Account.Infra.Tables;
using TransactionDomain = RSoft.Account.Core.Entities.Transaction;

namespace RSoft.Account.Infra.Extensions
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
            => Map(table, true);

        /// <summary>
        /// Maps table to entity
        /// </summary>
        /// <param name="table">Table entity to map</param>
        /// <param name="useLazy">Load related data</param>
        public static TransactionDomain Map(this Transaction table, bool useLazy)
        {
            TransactionDomain result = null;

            if (table != null)
            {

                result = new TransactionDomain(table.Id)
                {
                    CreatedOn = table.CreatedOn,
                    Year = table.Year,
                    Month = table.Month,
                    Date = table.Date,
                    Amount = table.Amount,
                    Comment = table.Comment
                };

                if (useLazy)
                {
                    //result.MapAuthor(table);
                    result.PaymentMethod = table.PaymentMethod?.Map(false);
                    result.Account = table.Account?.Map(false);
                }
                else
                {
                    result.PaymentMethod = new Core.Entities.PaymentMethod(table.PaymentMethodId);
                    result.Account = new Core.Entities.Account(table.AccountId);
                }

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
                    Date = entity.Date,
                    Amount = entity.Amount,
                    Comment = entity.Comment,
                    AccountId = entity.Account.Id,
                    PaymentMethodId= entity.PaymentMethod.Id,
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
                table.Year = entity.Year;
                table.Month = entity.Month;
                table.Date = entity.Date;
                table.Amount = entity.Amount;
                table.Comment = entity.Comment;
                table.AccountId = entity.Account.Id;
                table.PaymentMethodId = entity.PaymentMethod.Id;
                table.CreatedOn = entity.CreatedOn;
                table.CreatedBy = entity.CreatedAuthor.Id;
            }

            return table;

        }

    }

}