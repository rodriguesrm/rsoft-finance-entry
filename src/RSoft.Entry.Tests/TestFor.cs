﻿using AutoFixture;
using NUnit.Framework;
using RSoft.Entry.Tests.DependencyInjection;
using System;

namespace RSoft.Entry.Tests
{

    /// <summary>
    /// Abstract class to test objects
    /// </summary>
    /// <typeparam name="TSut">Object type to test</typeparam>
    public abstract class TestFor<TSut> : TestBase
        where TSut : class
    {

        #region Local objects/variables

        private Lazy<TSut> _lazySut;

        #endregion

        #region Constructors

        public TestFor()
        {
            ServiceInjection.BuildProvider();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Object instance to be tested
        /// </summary>
        protected TSut Target => _lazySut.Value;

        #endregion

        #region Local methods

        /// <summary>
        /// Create a instance for object to test
        /// </summary>
        protected virtual TSut CreateSut()
            => _fixture.Create<TSut>();

        /// <summary>
        /// Setup test
        /// </summary>
        /// <param name="fixture">Fixture object instance</param>
        protected virtual void Setup(IFixture fixture)
        {
        }

        #endregion

        #region Public methods

        [SetUp]
        public void SetUpTestsFor()
        {
            _lazySut = new Lazy<TSut>(CreateSut);
            Setup(_fixture);
        }

        #endregion

    }
}
