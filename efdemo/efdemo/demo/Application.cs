using efdemo.demo.Model;
using ETG.SABENTISpro.Application.Core.Module.Session.Model;
using ETG.SABENTISpro.Models.Core.Session;
using System;
using System.Collections.Generic;
using Z.EntityFramework.Plus;

namespace efdemo.demo
{
    /// <summary>
    /// 
    /// </summary>
    public class Application
    {
        protected ScopeBuilder sb;

        /// <summary>
        /// 
        /// </summary>
        public Application(CurrentMappings mappings)
        {
            this.sb = new ScopeBuilder(mappings);
        }

        /// <summary>
        /// Do the merge
        /// </summary>
        public void DoMerge()
        {
            // Bulk merge an entry
            // Reset the database
            using (var dcs2 = new DBModelSession())
            {
                using (var dcs = this.sb.GetContext<DBModelSession>())
                {
                    dcs.BulkMerge(new List<CORE_SESSION>()
                        {
                            new CORE_SESSION()
                            {
                                data = "mydata",
                                device = "mydevice",
                                expires = 1646464,
                                id = Guid.NewGuid(),
                                lastrefresh = 54844,
                                started = 4616161,
                                uuid = Guid.NewGuid()
                            }
                        },
                        options => options.ColumnPrimaryKeyExpression = r => r.id);

                    dcs.SaveChanges();

                    QueryFilterContextInterceptor interceptor = new QueryFilterContextInterceptor(dcs);

                    dcs.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetDatabase()
        {
            // Reset the database
            using (var dcs = this.sb.GetContext<DBModelSession>())
            {
                try
                {
                    dcs.Database.ExecuteSqlCommand("DROP TABLE [CORE_SESSIONS]");
                }
                catch
                {
                }

                dcs.Database.ExecuteSqlCommand(@"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CORE_SESSIONS]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CORE_SESSIONS](
	[id] [uniqueidentifier] NOT NULL,
	[started] [bigint] NULL,
	[lastrefresh] [bigint] NULL,
	[device] [nvarchar](255) NULL,
	[expires] [bigint] NOT NULL,
	[uuid] [uniqueidentifier] NOT NULL,
	[data] [nvarchar](max) NULL,
    [fk_core_user] [uniqueidentifier] NULL,
 CONSTRAINT [PK_N_CORE_SESSIONS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
;

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_N_CORE_SESSIONS_id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CORE_SESSIONS] ADD  CONSTRAINT [DF_N_CORE_SESSIONS_id]  DEFAULT (newsequentialid()) FOR [id]
END
;
");
                dcs.SaveChanges();
            }
        }
    }
}
