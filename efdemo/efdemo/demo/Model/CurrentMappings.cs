using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace efdemo.demo.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrentMappings
    {
        /// <summary>
        /// 
        /// </summary>
        protected Action<DbModelBuilder> Mappings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mappings"></param>
        public CurrentMappings(Action<DbModelBuilder> mappings)
        {
            this.Mappings = mappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void AddConfiguration(DbModelBuilder modelBuilder)
        {
            this.Mappings(modelBuilder);
        }
    }
}
