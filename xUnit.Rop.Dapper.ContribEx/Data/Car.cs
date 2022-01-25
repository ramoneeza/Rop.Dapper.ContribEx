using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    [Table("Automobiles")]
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Computed]
        public string Computed { get; set; }
    }
}