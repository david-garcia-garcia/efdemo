using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace efdemo.demo
{
    public class ConnectionBuilder
    {
        /// <summary>
        /// Get the connection string for this connection configuration.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.UserID = "sa";
            builder.Password = "Eqreurm2009";
            builder.InitialCatalog = "demoef";

            // Using ApplicationName here has transaction related implications.
            // For distributed transactions, connections must share the same application
            // name.
            builder.ApplicationName = "SabentisPRO";

            // Mars support has performance impact. Set it to false
            // and make the developers use database connections
            // properly. MARS is mostly there to please 
            // lazy developers, and we don't want those. Right?
            builder.MultipleActiveResultSets = false;

            // Utilizamos enlist=true porque las conexiones de EF y de ZZZ
            // no son compartidas, y para que usen la misma transacción
            // debemos usar esto.
            builder.Enlist = true;

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetConnection()
        {
            var cnn = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection();
            cnn.ConnectionString = GetConnectionString();
            return cnn;
        }
    }
}
