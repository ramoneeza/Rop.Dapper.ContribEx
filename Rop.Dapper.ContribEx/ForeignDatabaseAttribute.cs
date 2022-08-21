using System;

namespace Rop.Dapper.ContribEx
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class ForeignDatabaseAttribute : Attribute
    {
        public string Name { get; }

        public ForeignDatabaseAttribute(string databaseName)
        {
            Name = databaseName;
        }
    }
}