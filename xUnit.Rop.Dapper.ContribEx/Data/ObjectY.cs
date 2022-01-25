using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    [Table("ObjectY")]
    public class ObjectY
    {
        [ExplicitKey]
        public int ObjectYId { get; set; }
        public string Name { get; set; }
    }
}