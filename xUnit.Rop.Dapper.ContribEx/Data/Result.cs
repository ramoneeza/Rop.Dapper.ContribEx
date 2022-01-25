using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    [Table("Results")]
    public class Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}