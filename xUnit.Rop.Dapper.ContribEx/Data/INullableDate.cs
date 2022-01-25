using System;
using Dapper.Contrib.Extensions;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    public interface INullableDate
    {
        [Key]
        int Id { get; set; }
        DateTime? DateValue { get; set; }
    }
}