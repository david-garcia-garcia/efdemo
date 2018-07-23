using ETG.SABENTISpro.Application.Core.Module.Session.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace efdemo.demo.Model
{
    /// <summary>
    /// 
    /// </summary>
    class GlobalDBConfiguration : DbConfiguration
    {
        /// <summary>
        /// Get instance of GlobalDBConfiguration
        /// </summary>
        public GlobalDBConfiguration()
        {
            this.SetDatabaseInitializer<DBModelSession>(new NullDatabaseInitializer<DBModelSession>());
        }
    }
}
