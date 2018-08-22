using ETG.SABENTISpro.Models.Core.Session;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using efdemo.demo.Model;

namespace ETG.SABENTISpro.Application.Core.Module.Session.Model
{
    /// <summary>
    /// Aunque esto está en core....
    /// 
    /// El sistema de locks está 100% descoplado del resto de cosas del sistema,
    /// luego tiene sentido que tenga su propio modelo.
    /// </summary>
    [DbConfigurationType(typeof(GlobalDBConfiguration))]
    public partial class DBModelSession : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="compiledModel"></param>
        /// <param name="ownsDataContext"></param>
        public DBModelSession(System.Data.SqlClient.SqlConnection connection, DbCompiledModel compiledModel, bool ownsDataContext)
                    : base(connection, compiledModel, ownsDataContext)
        {
        }

        public DBModelSession() : base ()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Data.Entity.DbSet<CORE_SESSION> CORE_SESSION { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Data.Entity.DbSet<CORE_USER> CORE_USER { get; set; }
    }
}
