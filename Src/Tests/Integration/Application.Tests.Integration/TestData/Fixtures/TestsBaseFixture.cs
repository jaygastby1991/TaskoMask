﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskoMask.Infrastructure.Data.ReadModel.DataProviders;
using TaskoMask.Infrastructure.Data.WriteModel.DataProviders;
using TaskoMask.Presentation.Framework.Web.Configuration.Startup;

namespace TaskoMask.Application.Tests.Integration.TestData.Fixtures
{
    /// <summary>
    /// Each test class must have its own fixture and each fixture initialize and dispose a unique database
    /// So, we have control over parallel test run and lower cost for creating database
    /// ------------------* But *-----------------------------------------------
    /// If you want TestsBaseFixture to be initialized and disposed for each test method in the class
    /// You just need to  Inherit from TestsBaseFixture for that class
    /// So the TestsBaseFixture initialize before each test method and then dispose after that test run
    /// ------------------* But *-----------------------------------------------
    /// If you want to share TestsBaseFixture for all test methods in a Test Class
    /// You just need to Inherit from IClassFixture<TestsBaseFixture> for that class
    /// And get TestsBaseFixture as a parameter in test class constructor
    /// So the TestsBaseFixture initialize before all test methods in that test class and then dispose after all tests run
    /// ------------------* But *-----------------------------------------------
    /// If you want TestsBaseFixture to be initialized and disposed for each test method in the class
    /// You just need to  Inherit from TestsBaseFixture for that class
    /// So the TestsBaseFixture initialize before each test method and then dispose after that test run
    /// ------------------* But *-----------------------------------------------
    /// If you want to share TestsBaseFixture for all test methods in some Test Classes
    /// You just need to make a new class inherited from ICollectionFixture<OwnerCollectionFixture>
    /// Then apply [CollectionDefinition("my Collection Fixture")] attribute for that new class
    /// And then apply [Collection("my Collection Fixture")] attribute for those test classes you want to share the fixture between
    /// And get TestsBaseFixture as a parameter in each test class constructor
    /// So the TestsBaseFixture initialize before all test methods in all test classes and then dispose after all tests run
    /// </summary>
    public abstract class TestsBaseFixture : IDisposable
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbNameSuffix">To make a unique database for each fixture</param>
        public TestsBaseFixture(string dbNameSuffix)
        {
            _serviceProvider = GetServiceProvider(dbNameSuffix);
            InitializeDatabases();
            SeedSampleData();
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Get required services for each service
        /// </summary>
        public T GetRequiredService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }



        /// <summary>
        /// Seed some sample data in the database for the fixture
        /// </summary>
        public void SeedSampleData()
        {
            WriteDbSeedData.SeedSampleData(_serviceProvider);
            ReadDbSeedData.SyncSampleData(_serviceProvider);
        }



        #endregion

        #region Private Methods


        /// <summary>
        /// 
        /// </summary>
        private static IServiceProvider GetServiceProvider(string dbNameSuffix)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                                //Copy from AdminPanel project during the build event
                                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                                .AddJsonFile("appsettings.Development.json", reloadOnChange: true, optional: false)
                                .AddInMemoryCollection(new[]
                                {
                                   new KeyValuePair<string,string>("Mongo:Write:Database", $"TaskoMask_WriteDB_Test_{dbNameSuffix}"),
                                   new KeyValuePair<string,string>("Mongo:Read:Database", $"TaskoMask_ReadDB_Test_{dbNameSuffix}"),
                                })
                                .Build();

            services.AddSingleton<IConfiguration>(provider => { return configuration; });

            services.AddCommonConfigureServices(configuration);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }



        /// <summary>
        /// 
        /// </summary>
        private void InitializeDatabases()
        {
            WriteDbInitialization.Initial(_serviceProvider);
            ReadDbInitialization.Initial(_serviceProvider);
            WriteDbSeedData.SeedEssentialData(_serviceProvider);
        }



        /// <summary>
        /// 
        /// </summary>
        private void DropDatabases()
        {
            WriteDbInitialization.DropDatabase(_serviceProvider);
            ReadDbInitialization.DropDatabase(_serviceProvider);
        }



        #endregion

        #region Dispose


        /// <summary>
        /// Dispose all resources that fixture used for tests
        /// </summary>
        public void Dispose()
        {
            DropDatabases();
        }


        #endregion
    }
}