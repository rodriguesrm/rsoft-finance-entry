﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using RSoft.Lib.Common.Contracts;
using RSoft.Lib.Common.Abstractions;
using RSoft.Finance.Contracts.Enum;
using CategoryDomain = RSoft.Account.Core.Entities.Category;
using AccountDomain = RSoft.Account.Core.Entities.Account;
using AccrualPeriodDomain = RSoft.Account.Core.Entities.AccrualPeriod;
using TransactionDomain = RSoft.Account.Core.Entities.Transaction;
using UserDomain = RSoft.Account.Core.Entities.User;
using RSoft.Account.Core.Services;
using RSoft.Account.NTests.Stubs;
using RSoft.Lib.Common.Contracts.Web;
using System.Diagnostics.CodeAnalysis;

namespace RSoft.Account.NTests.DependencyInjection
{

    /// <summary>
    /// Services injection static class
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Test class should not be considered in test coverage.")]
    public static class ServiceInjection
    {

        #region Local Objects/Variables


        private static IServiceCollection _serviceCollection = null;
        private static IServiceProvider _serviceProvider = null;

        #endregion

        #region Properties


        public static IServiceCollection ServiceCollection
        {
            get
            {
                if (_serviceCollection == null)
                {
                    _serviceCollection = new ServiceCollection()

                        .AddScoped<IStringLocalizer<CategoryDomain>, StringLocalizerStub<CategoryDomain>>()
                        .AddScoped<IStringLocalizer<AccountDomain>, StringLocalizerStub<AccountDomain>>()
                        .AddScoped<IStringLocalizer<SimpleStringValidationContract>, StringLocalizerStub<SimpleStringValidationContract>>()
                        .AddScoped<IStringLocalizer<RequiredValidationContract<Guid?>>, StringLocalizerStub<RequiredValidationContract<Guid?>>>()
                        .AddScoped<IStringLocalizer<AccrualPeriodDomain>, StringLocalizerStub<AccrualPeriodDomain>>()
                        .AddScoped<IStringLocalizer<EnumCastFromIntegerValidationContract<PaymentTypeEnum>>, StringLocalizerStub<EnumCastFromIntegerValidationContract<PaymentTypeEnum>>>()
                        .AddScoped<IStringLocalizer<TransactionDomain>, StringLocalizerStub<TransactionDomain>>()
                        .AddScoped<IStringLocalizer<PastDateValidationContract>, StringLocalizerStub<PastDateValidationContract>>()
                        .AddScoped<IStringLocalizer<EnumCastFromIntegerValidationContract<TransactionTypeEnum>>, StringLocalizerStub<EnumCastFromIntegerValidationContract<TransactionTypeEnum>>>()
                        .AddScoped<IStringLocalizer<UserDomain>, StringLocalizerStub<UserDomain>>()
                        .AddScoped<IStringLocalizer<FullNameValidationContract>, StringLocalizerStub<FullNameValidationContract>>()
                        .AddScoped<IStringLocalizer<TransactionDomainService>, StringLocalizerStub<TransactionDomainService>>()
                        .AddScoped<IAuthenticatedUser, AuthenticatedUserStub>()

                        .AddLogging();
                }
                return _serviceCollection;

            }
        }

        public static IServiceProvider GetServiceProvider
        {
            get => _serviceProvider;
        }

        #endregion

        #region Public methods

        public static IServiceProvider BuildProvider()
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = ServiceCollection.BuildServiceProvider();
                ServiceActivator.Configure(GetServiceProvider);
            }
            return _serviceProvider;
        }

        #endregion

    }
}
