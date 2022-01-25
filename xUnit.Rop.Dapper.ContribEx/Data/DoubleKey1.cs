using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    public class DoubleKey1
    {
        [ExplicitKey]
        public string Key1 { get; set; }
        [ExplicitKey]
        public string Key2 { get; set; }

        public string Data { get; set; }
    }
    public class DoubleKey2
    {
        [Key]
        public int Key1 { get; set; }
        [Key]
        public int Key2 { get; set; }

        public string Data { get; set; }
    }
}
