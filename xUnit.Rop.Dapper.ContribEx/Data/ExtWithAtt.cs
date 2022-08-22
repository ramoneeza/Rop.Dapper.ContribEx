using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Rop.Dapper.ContribEx;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    [Table("Intranet.dbo.Permisos")]
    public class ExtWithAttTable
    {
        public string Aplicacion { get; init; } = "";
        public string Cuenta { get; init; } = "";
        public string Role { get; init; } = "";
    }
    [ForeignDatabase("Intranet")]
    [Table("Permisos")]
    public class ExtWithAttTable2
    {
        public string Aplicacion { get; init; } = "";
        public string Cuenta { get; init; } = "";
        public string Role { get; init; } = "";
    }
    [ForeignDatabase("Intranet")]
    [Table("Permisos")]
    public class ExtWithKey1
    {
        [ExplicitKey]
        public string Cuenta { get; init; } = "";
        public string Role { get; init; } = "";
    }
    [Table("Intranet.dbo.Permisos")]
    public class ExtWithKey2
    {
        [ExplicitKey]
        public string Cuenta { get; init; } = "";
        public string Role { get; init; } = "";
    }
}
