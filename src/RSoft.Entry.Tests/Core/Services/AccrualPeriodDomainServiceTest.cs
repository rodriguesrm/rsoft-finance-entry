﻿using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.Extensions.Localization;
using NUnit.Framework;
using RSoft.Entry.Core.Entities;
using RSoft.Entry.Core.Ports;
using RSoft.Entry.Core.Services;
using RSoft.Entry.Infra;
using RSoft.Entry.Infra.Providers;
using RSoft.Entry.Tests.Extensions;
using RSoft.Entry.Tests.Stubs;
using RSoft.Lib.Common.Models;
using RSoft.Lib.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccrualPeriodTable = RSoft.Entry.Infra.Tables.AccrualPeriod;

namespace RSoft.Entry.Tests.Core.Services
{

    public class AccrualPeriodDomainServiceTest : TestFor<AccrualPeriodDomainService>
    {

        #region Local objects/variables

        private EntryContext _dbContext;

        #endregion

        #region Constructors

        public AccrualPeriodDomainServiceTest() : base() { }

        #endregion

        #region Overrides

        protected override void Setup(IFixture fixture)
        {
            _fixture.WithInMemoryDatabase(out _dbContext);
            _fixture.Customizations.Add(new TypeRelay(typeof(IAccrualPeriodProvider), typeof(AccrualPeriodProvider)));
            _fixture.Customizations.Add(new TypeRelay(typeof(ITransactionProvider), typeof(TransactionProvider)));
            _fixture.Customizations.Add(new TypeRelay(typeof(IStringLocalizer<AccrualPeriodDomainService>), typeof(StringLocalizerStub<AccrualPeriodDomainService>)));
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Load initial categories
        /// </summary>
        /// <param name="year">Year output</param>
        /// <param name="month">Mont output</param>
        private void LoadInitialAccrualPeriod(out int year, out int month)
        {

            DateTime currentMonth = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            DateTime nextMonth = currentMonth.AddMonths(1);

            AccrualPeriodTable table = _dbContext.AccrualPeriods.Where(a => a.Year == currentMonth.Year && a.Month == currentMonth.Month).FirstOrDefault();
            if (table == null)
            {
                AccrualPeriodTable rowA = _fixture.CreateAccrualPeriod(currentMonth.Year, currentMonth.Month, 1000);
                AccrualPeriodTable rowB = _fixture.CreateAccrualPeriod(nextMonth.Year, nextMonth.Month, 0);
                _fixture.WithSeedData(_dbContext, new List<AccrualPeriodTable>() { rowA, rowB });
                year = currentMonth.Year;
                month = currentMonth.Month;
            }
            else
            {
                year = table.Year;
                month = table.Month;
            }
        }

        #endregion

        #region Tests

        [Test]
        public async Task AddAsync_ReturnEntitySaved()
        {
            int year = DateTime.UtcNow.AddMonths(2).Year;
            int month = DateTime.UtcNow.AddMonths(2).Month;
            AccrualPeriod accrualPeriod = _fixture.Build<AccrualPeriod>()
                .With(a => a.Year, year)
                .With(a => a.Month, month)
                .With(a => a.OpeningBalance, 0)
                .Create();
            AccrualPeriod result = await Target.AddAsync(accrualPeriod, default);
            Assert.IsTrue(result.Valid);
            AccrualPeriodTable check = _dbContext.AccrualPeriods.Find(year, month);
            Assert.NotNull(check);
            Assert.AreEqual(accrualPeriod.Year, check.Year);
            Assert.AreEqual(accrualPeriod.Month, check.Month);
            Assert.AreEqual(accrualPeriod.OpeningBalance, check.OpeningBalance);
        }

        [Test]
        public async Task GetById_ReturnEntity()
        {
            LoadInitialAccrualPeriod(out int year, out int month);
            AccrualPeriodTable table = _dbContext.AccrualPeriods.Find(year, month);
            AccrualPeriod result = await Target.GetByKeyAsync(year, month, default);
            Assert.NotNull(result);
            Assert.AreEqual(table.Year, result.Year);
            Assert.AreEqual(table.Month, result.Month);
            Assert.AreEqual(table.OpeningBalance, result.OpeningBalance);
            Assert.AreEqual(table.IsClosed, result.IsClosed);
        }

        [Test]
        public async Task GetAllAccrualPeriod_ReturnEntityList()
        {
            LoadInitialAccrualPeriod(out int year, out int month);
            DateTime date1 = new(year, month, 1);
            DateTime date2 = date1.AddMonths(1);
            IEnumerable<AccrualPeriod> result = await Target.GetAllAsync(default);
            Assert.GreaterOrEqual(result.Count(), 2);
            AccrualPeriodTable accrualPeriodA = _dbContext.AccrualPeriods.First(a => a.Year == date1.Year && a.Month == date1.Month);
            AccrualPeriodTable accrualPeriodB = _dbContext.AccrualPeriods.First(a => a.Year == date2.Year && a.Month == date2.Month);
            Assert.True(result.Any(x => x.Year == date1.Year && x.Month == date1.Month));
            Assert.True(result.Any(x => x.Year == date2.Year && x.Month == date2.Month));
        }

        [Test]
        public void UpdateAccrualPeriod_SuccessOnUpdate()
        {
            DateTime date = DateTime.UtcNow.AddMonths(5);
            AccrualPeriodTable oldTableRow = _fixture.CreateAccrualPeriod(date.Year, date.Month, 0);
            _fixture.WithSeedData(_dbContext, new AccrualPeriodTable[] { oldTableRow });
            AccrualPeriod accrualPeriod = new(date.Year, date.Month) { OpeningBalance = 1000, ChangedAuthor = One<AuthorNullable<Guid>>() };
            accrualPeriod = Target.Update(date.Year, date.Month, accrualPeriod);
            AccrualPeriodTable check = _dbContext.AccrualPeriods.Where(a => a.Year == date.Year && a.Month == date.Month).FirstOrDefault();
            Assert.NotNull(accrualPeriod);
            Assert.NotNull(check);
            Assert.AreEqual(check.Year, accrualPeriod.Year);
            Assert.AreEqual(check.Month, accrualPeriod.Month);
            Assert.AreEqual(check.OpeningBalance, accrualPeriod.OpeningBalance);
            _dbContext.AccrualPeriods.Remove(check);
            _dbContext.SaveChanges();
        }

        [Test]
        public void UpdateAccrualPeriod_WithInvalidEntity_ReturnNotifications()
        {
            AccrualPeriod entity = One<AccrualPeriod>();
            Target.Update(entity.Year, entity.Month, entity);
            Assert.True(entity.Invalid);
            Assert.True(entity.Notifications.Count > 0);
        }

        [Test]
        public void UpdateNonExistingAccrualPeriod_ThrowException()
        {
            DateTime date = DateTime.UtcNow.AddMonths(-10);
            AccrualPeriod accrualPeriod = new(date.Year, date.Month);
            void DoUpdate()
            {
                Target.Update(accrualPeriod.Year, accrualPeriod.Month, accrualPeriod);
            }
            Assert.Throws<InvalidOperationException>(DoUpdate);
        }

        [Test]
        public void DeleteAccrualPeriod_SuccessOnDelete()
        {
            DateTime date = DateTime.UtcNow.AddMonths(5);
            AccrualPeriodTable tableRow = _fixture.CreateAccrualPeriod(date.Year, date.Month, 7000);
            _fixture.WithSeedData(_dbContext, new AccrualPeriodTable[] { tableRow });
            Target.Delete(date.Year, date.Month);
            _dbContext.SaveChanges();
            AccrualPeriodTable check = _dbContext.AccrualPeriods.Where(a => a.Year == date.Year && a.Month == date.Month).FirstOrDefault();
            Assert.Null(check);
        }

        [Test]
        public async Task CloseAccrualPeriod_ProcessSuccess()
        {

            DateTime date = DateTime.UtcNow.AddMonths(13);
            AccrualPeriodTable tableRow = _fixture.CreateAccrualPeriod(date.Year, date.Month, 1000);
            tableRow.Transactions.Add(_fixture.CreateTransaction(date.Year, date.Month, 500, Finance.Contracts.Enum.TransactionTypeEnum.Credit));
            tableRow.Transactions.Add(_fixture.CreateTransaction(date.Year, date.Month, 500, Finance.Contracts.Enum.TransactionTypeEnum.Credit));
            tableRow.Transactions.Add(_fixture.CreateTransaction(date.Year, date.Month, 500, Finance.Contracts.Enum.TransactionTypeEnum.Credit));
            tableRow.Transactions.Add(_fixture.CreateTransaction(date.Year, date.Month, 150, Finance.Contracts.Enum.TransactionTypeEnum.Debt));
            tableRow.Transactions.Add(_fixture.CreateTransaction(date.Year, date.Month, 150, Finance.Contracts.Enum.TransactionTypeEnum.Debt));
            tableRow.Transactions.Add(_fixture.CreateTransaction(date.Year, date.Month, 150, Finance.Contracts.Enum.TransactionTypeEnum.Debt));
            _fixture.WithSeedData(_dbContext, new AccrualPeriodTable[] { tableRow });

            SimpleOperationResult result = await Target.ClosePeriodAsync(date.Year, date.Month);
            _dbContext.SaveChanges();
            Assert.IsTrue(result.Success);
            AccrualPeriodTable tableCheck = _dbContext.AccrualPeriods.FirstOrDefault(a => a.Year == date.Year && a.Month == date.Month);
            Assert.NotNull(tableCheck);
            Assert.AreEqual(1500, tableCheck.TotalCredits);
            Assert.AreEqual(450, tableCheck.TotalDebts);

        }

        [Test]
        public async Task CloseAccrualPeriod_WhenPeriodNotExists_ProcessFailReturnErroMessage()
        {

            DateTime date = DateTime.UtcNow.AddMonths(-9);
            SimpleOperationResult result = await Target.ClosePeriodAsync(date.Year, date.Month);
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Errors.Any(e => e.Value == "ACCRUAL_PERIOD_NOT_FOUND"));

        }

        [Test]
        public async Task CloseAccrualPeriod_WhenPeriodAlreadyClosed_ProcessFailReturnErroMessage()
        {
            DateTime date = DateTime.UtcNow.AddMonths(-9);
            AccrualPeriodTable tableRow = _fixture.CreateAccrualPeriod(date.Year, date.Month, 0);
            tableRow.IsClosed = true;
            tableRow.UserIdClosed = AuthenticatedUserStub.UserAdminId;
            _fixture.WithSeedData(_dbContext, new AccrualPeriodTable[] { tableRow });
            SimpleOperationResult result = await Target.ClosePeriodAsync(date.Year, date.Month);
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Errors.Any(e => e.Value == "ACCRUAL_PERIOD_ALREADY_CLOSED"));
        }

        #endregion

    }
}
