using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace efdemo.demo.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ScopeBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        protected CurrentMappings Mappings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mappings"></param>
        /// <param name="connection"></param>
        public ScopeBuilder(CurrentMappings mappings)
        {
            this.Mappings = mappings;
        }

        /// <summary>
        /// Generate a compiled model with current mapping rules
        /// </summary>
        /// <returns></returns>
        public DbCompiledModel GenerateCompiledModel(DbConnection connection)
        {
            // The connection factory is also in charge of managing
            // the compiled DBModelBuilders
            var modelBuilder = new DbModelBuilder();
            this.Mappings.AddConfiguration(modelBuilder);
            var model = modelBuilder.Build(connection);
            var compiledModel = model.Compile();
            return compiledModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContextType"></typeparam>
        /// <returns></returns>
        public TContextType GetContext<TContextType>()
            where TContextType : DbContext
        {
            try
            {
                var connection = ConnectionBuilder.GetConnection();

                return (TContextType) Activator.CreateInstance(
                    typeof(TContextType),
                    connection, 
                    this.GenerateCompiledModel(connection), 
                    true);
            }
            catch (TargetInvocationException ex)
            {
                // Rethrow the inner exception as that is what we are actually interested in.
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }
    }
}
