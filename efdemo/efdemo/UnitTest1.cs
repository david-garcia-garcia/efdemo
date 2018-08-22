using System;
using System.Data.Entity;
using efdemo.demo;
using efdemo.demo.Model;
using EntityFramework.DynamicFilters;
using ETG.SABENTISpro.Application.Core.Kernel;
using ETG.SABENTISpro.Application.Core.Module.Session.Model;
using ETG.SABENTISpro.Models.Core.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace efdemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CurrentMappings currentMappings;
            Application app;

            // Here the CORE_USER entity is NOT being ignored
            currentMappings = new CurrentMappings((modelBuilder) =>
            {
                modelBuilder.Configurations.Add(new CORE_SESSIONConfiguration());
                modelBuilder.Configurations.Add(new CORE_USERConfiguration());
                modelBuilder.Filter("test", (CORE_USER b, bool isDeleted) => true, () => false);
            });

            GlobalDBConfiguration config = new GlobalDBConfiguration();
            DbConfiguration.SetConfiguration(config);

            app = new Application(currentMappings);
            app.ResetDatabase();
            app.DoMerge();

            // Here we ignore both the entity and the field that has the FK
            currentMappings = new CurrentMappings((modelBuilder) =>
            {
                modelBuilder.Configurations.Add(new CORE_SESSIONConfiguration());
                modelBuilder.Entity<CORE_SESSION>().Ignore(t => t.fk_core_user);
                modelBuilder.Ignore<CORE_USER>();
            });

            ZzzUtils.ClearZzzAndEfCaches(ConnectionBuilder.GetConnectionString());

            var dcs = new DBModelSession();

            app = new Application(currentMappings);
            app.DoMerge();
        }
    }
}
