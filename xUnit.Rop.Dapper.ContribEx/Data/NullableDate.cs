using System;

namespace xUnit.Rop.Dapper.ContribEx.Data
{
    public class NullableDate : INullableDate
    {
        public int Id { get; set; }
        public DateTime? DateValue { get; set; }
    }
}