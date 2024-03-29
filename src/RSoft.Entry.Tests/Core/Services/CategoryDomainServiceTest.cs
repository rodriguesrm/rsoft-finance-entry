﻿using AutoFixture;
using AutoFixture.Kernel;
using NUnit.Framework;
using RSoft.Entry.Core.Entities;
using RSoft.Entry.Core.Ports;
using RSoft.Entry.Core.Services;
using RSoft.Entry.Infra;
using RSoft.Entry.Infra.Providers;
using RSoft.Entry.Tests.Extensions;
using RSoft.Lib.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CategoryTable = RSoft.Entry.Infra.Tables.Category;

namespace RSoft.Entry.Tests.Core.Services
{

    public class CategoryDomainServiceTest : TestFor<CategoryDomainService>
    {

        #region Local objects/variables

        private const string _categoryAName = "VEHICLES";
        private const string _categoryBName = "LEISURE";

        private EntryContext _dbContext;

        #endregion

        #region Constructors

        public CategoryDomainServiceTest() : base() { }

        #endregion

        #region Overrides

        protected override void Setup(IFixture fixture)
        {
            _fixture.WithInMemoryDatabase(out _dbContext);
            _fixture.Customizations.Add(new TypeRelay(typeof(ICategoryProvider), typeof(CategoryProvider)));

        }

        #endregion

        #region Local methods

        /// <summary>
        /// Load initial categories
        /// </summary>
        /// <param name="categoryId">Category id output</param>
        private void LoadInitialCategory(out Guid categoryId)
        {
            CategoryTable table = _dbContext.Categories.Where(c => c.Name == _categoryAName).FirstOrDefault();
            if (table == null)
            {
                CategoryTable rowA = _fixture.CreateCategory(_categoryAName);
                CategoryTable rowB = _fixture.CreateCategory(_categoryBName);
                _fixture.WithSeedData(_dbContext, new List<CategoryTable>() { rowA, rowB });
                categoryId = rowA.Id;
            }
            else
            {
                categoryId = table.Id;
            }
        }

        #endregion

        #region Tests

        [Test]
        public async Task AddAsync_ReturnEntitySaved()
        {
            Category category = _fixture.Build<Category>()
                .With(c => c.Name, "CATEGORY TEST")
                .With(c => c.CreatedAuthor, One<Author<Guid>>())
                .With(c => c.ChangedAuthor, One<AuthorNullable<Guid>>())
                .Create();
            Category result = await Target.AddAsync(category, default);
            Assert.IsTrue(result.Valid);
            CategoryTable check = _dbContext.Categories.Find(result.Id);
            Assert.NotNull(check);
            Assert.AreEqual(category.Id, check.Id);
            Assert.AreEqual(category.Name, check.Name);
        }

        [Test]
        public async Task GetById_ReturnEntity()
        {
            LoadInitialCategory(out Guid categoryId);
            CategoryTable table = _dbContext.Categories.Find(categoryId);
            Category result = await Target.GetByKeyAsync(categoryId, default);
            Assert.NotNull(result);
            Assert.AreEqual(table.Name, result.Name);
        }

        [Test]
        public async Task GetAllCategory_ReturnEntityList()
        {
            LoadInitialCategory(out _);
            IEnumerable<Category> result = await Target.GetAllAsync(default);
            Assert.GreaterOrEqual(result.Count(), 2);
            CategoryTable categoryA = _dbContext.Categories.First(c => c.Name == _categoryAName);
            CategoryTable categoryB = _dbContext.Categories.First(c => c.Name == _categoryBName);
            Assert.True(result.Any(x => x.Id == categoryA.Id));
            Assert.True(result.Any(x => x.Id == categoryB.Id));
        }

        [Test]
        public void UpdateCategory_SuccessOnUpdate()
        {
            string oldName = "Category X";
            string newName = "New Category";
            CategoryTable oldTableRow = _fixture.CreateCategory(oldName);
            _fixture.WithSeedData(_dbContext, new CategoryTable[] { oldTableRow });
            Category category = new(oldTableRow.Id) { Name = newName };
            category = Target.Update(category.Id, category);
            CategoryTable check = _dbContext.Categories.Where(c => c.Id == category.Id).FirstOrDefault();
            Assert.NotNull(category);
            Assert.NotNull(check);
            Assert.AreEqual(check.Name, category.Name);
            Assert.AreEqual(check.Name, newName);
            Assert.AreEqual(category.Name, newName);
        }

        [Test]
        public void DeleteCategory_SuccessOnDelete()
        {
            CategoryTable tableRow = _fixture.CreateCategory("CATEGORY TO_REMOVE");
            Guid categoryId = tableRow.Id;
            _fixture.WithSeedData(_dbContext, new CategoryTable[] { tableRow });
            Target.Delete(categoryId);
            _dbContext.SaveChanges();
            CategoryTable check = _dbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
            Assert.Null(check);
        }

        #endregion

    }
}
