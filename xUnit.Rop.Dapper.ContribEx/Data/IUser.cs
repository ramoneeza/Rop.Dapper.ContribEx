using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    public interface IUser
    {
        [Key]
        int Id { get; set; }
        string Name { get; set; }
        int Age { get; set; }
    }
}