// <auto-generated />
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETG.SABENTISpro.Models.Core.Session
{

    // CORE_SESSIONS
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.31.0.0")]
    public partial class CORE_SESSIONConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CORE_SESSION>
    {
        public CORE_SESSIONConfiguration()
            : this("dbo")
        {
        }

        public CORE_SESSIONConfiguration(string schema)
        {
            Property(x => x.data).IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}