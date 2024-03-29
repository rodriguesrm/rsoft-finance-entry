﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using RSoft.Entry.Application.Handlers;
using RSoft.Entry.Contracts.Commands;
using RSoft.Entry.Contracts.Models;
using RSoft.Entry.Core.Entities;
using RSoft.Entry.Core.Ports;
using RSoft.Lib.Design.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Entry.Tests.Application.Handlers
{
    public class ListTransactionCommandHandlerTest : TestFor<ListTransactionCommandHandler>
    {

        #region Constructors

        public ListTransactionCommandHandlerTest() : base() { }

        #endregion

        #region Overrides

        protected override void Setup(IFixture fixture)
        {
            _fixture.Customize<Transaction>(c => c.FromFactory(() => new Transaction() { Date = DateTime.UtcNow }));
            Mock<ITransactionDomainService> domainService = new();
            domainService
                .Setup(m => m.GetByFilterAsync(It.IsAny<IListTransactionFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    IEnumerable<Transaction> entities = new List<Transaction>() { One<Transaction>(), One<Transaction>(), One<Transaction>()};
                    return entities;
                });
            _fixture.Inject(domainService.Object);
        }

        #endregion

        #region Tests

        [Test]
        public async Task HandleMediatorCommand_ProcessSuccess()
        {

            ListTransactionCommand command = new();
            CommandResult<IEnumerable<TransactionDto>> result = await Target.Handle(command, default);
            Assert.NotNull(result);
            Assert.True(result.Success);
            IEnumerable<TransactionDto> dtos = result.Response;
            Assert.NotNull(dtos);
            Assert.AreEqual(3, dtos.Count());
        }

        #endregion

    }
}
