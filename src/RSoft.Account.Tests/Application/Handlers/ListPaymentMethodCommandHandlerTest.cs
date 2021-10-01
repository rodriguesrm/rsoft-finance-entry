﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using RSoft.Account.Application.Handlers;
using RSoft.Account.Contracts.Commands;
using RSoft.Account.Contracts.Models;
using RSoft.Account.Core.Entities;
using RSoft.Account.Core.Ports;
using RSoft.Account.Tests.DependencyInjection;
using RSoft.Lib.Design.Application.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RSoft.Account.Tests.Application.Handlers
{
    public class ListPaymentMethodCommandHandlerTest : TestFor<ListPaymentMethodCommandHandler>
    {

        #region Constructors

        public ListPaymentMethodCommandHandlerTest()
        {
            ServiceInjection.BuildProvider();
        }

        #endregion

        #region Overrides

        protected override void Setup(IFixture fixture)
        {
            Mock<IPaymentMethodDomainService> domainService = new();
            domainService
                .Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {

                    IEnumerable<PaymentMethod> entities = new List<PaymentMethod>() { One<PaymentMethod>(), One<PaymentMethod>(), One<PaymentMethod>()};
                    return entities;
                });
            _fixture.Inject(domainService.Object);
        }

        #endregion

        #region Tests

        [Test]
        public async Task HandleMediatorCommand_ProcessSuccess()
        {

            ListPaymentMethodCommand command = new();
            CommandResult<IEnumerable<PaymentMethodDto>> result = await Sut.Handle(command, default);
            Assert.NotNull(result);
            Assert.True(result.Success);
            IEnumerable<PaymentMethodDto> dtos = result.Response;
            Assert.NotNull(dtos);
            Assert.AreEqual(3, dtos.Count());
        }

        #endregion

    }
}
