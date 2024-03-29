﻿using TransactionDomain = RSoft.Entry.Core.Entities.Transaction;
using EntryDomain = RSoft.Entry.Core.Entities.Entry;
using System;
using System.Linq;
using NUnit.Framework;
using RSoft.Finance.Contracts.Enum;
using RSoft.Entry.Core.Entities;
using RSoft.Lib.Common.ValueObjects;

namespace RSoft.Entry.Tests.Core.Entities
{

    public class TransactionTest : TestFor<TransactionDomain>
    {

        #region Constructors

        public TransactionTest() : base() { }

        #endregion

        #region Tests

        [Test]
        public void CreateTransactionInstance_ResultSuccess()
        {

            TransactionDomain transaction1 = new();
            TransactionDomain transaction2 = new(Guid.NewGuid());
            TransactionDomain transaction3 = new("1d899c45-6d83-41d4-afdc-e004a6250559");

            Assert.NotNull(transaction1);
            Assert.NotNull(transaction2);
            Assert.NotNull(transaction3);

            Assert.Null(transaction1.Comment);
            Assert.AreEqual(0, transaction1.Year);
            Assert.AreEqual(0, transaction1.Month);

        }

        [Test]
        public void ValidateTransactionWhenDataIsInvalid_ResultInvalidTrue()
        {
            TransactionDomain transaction = new();
            transaction.Validate();
            Assert.True(transaction.Invalid);
            Assert.AreEqual(5, transaction.Notifications.Count);
            Assert.True(transaction.Notifications.Any(n => n.Message == "GREATER_THAN_ZERO"));
            Assert.True(transaction.Notifications.Any(n => n.Message == "DATE_REQUIRED"));
            Assert.True(transaction.Notifications.Any(n => n.Message == "FIELD_REQUIRED"));
            Assert.True(transaction.Notifications.Any(n => n.Message == "ENTRY_REQUIRED"));
            Assert.True(transaction.Notifications.Any(n => n.Message == "PAYMENTMETHOD_REQUIRED"));
        }

        [Test]
        public void ValidateTransactionWhenDataIsValid_ResultValidTrue()
        {
            float amount = 450f;
            string comment = "COMMENT TEST";
            DateTime date = DateTime.UtcNow.AddMinutes(-1);
            TransactionDomain transaction = new()
            {
                Date = date,
                TransactionType = TransactionTypeEnum.Debt,
                Amount = amount,
                Comment = comment,
                PaymentMethod = new PaymentMethod(Guid.NewGuid()) { Name = "PAYMENT_METHOD_NAME" },
                Entry = new EntryDomain(Guid.NewGuid()) { Name = "ENTRY_NAME" },
                CreatedAuthor = One<Author<Guid>>()
            };
            transaction.Validate();
            Assert.True(transaction.Valid);
            Assert.AreEqual(transaction.Year, date.Year);
            Assert.AreEqual(transaction.Month, date.Month);
        }

        #endregion

    }
}
