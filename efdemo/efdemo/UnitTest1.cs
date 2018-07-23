using System;
using System.Data.Entity;
using efdemo.demo;
using efdemo.demo.Model;
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

            currentMappings = new CurrentMappings((modelBuilder) =>
            {
                modelBuilder.Configurations.Add(new CORE_SESSIONConfiguration());
            });

            GlobalDBConfiguration config = new GlobalDBConfiguration();
            DbConfiguration.SetConfiguration(config);

            app = new Application(currentMappings);
            app.ResetDatabase();
            app.DoMerge();

            currentMappings = new CurrentMappings((modelBuilder) =>
            {
                modelBuilder.Configurations.Add(new CORE_SESSIONConfiguration());
                modelBuilder.Entity<CORE_SESSION>().Ignore(t => t.uuid);
            });

            ZzzUtils.ClearZzzAndEfCaches(ConnectionBuilder.GetConnectionString());
            var dcs = new DBModelSession();
            app = new Application(currentMappings);
            app.DoMerge();
        }
    }
}
