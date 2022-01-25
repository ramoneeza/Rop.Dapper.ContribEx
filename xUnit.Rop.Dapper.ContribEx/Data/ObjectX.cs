using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
     [Table("ObjectX")]
    public class ObjectX
    {
        [ExplicitKey]
        public string ObjectXId { get; set; }
        public string Name { get; set; }
    }
}
