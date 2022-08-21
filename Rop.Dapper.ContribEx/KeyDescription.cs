using System;
using System.Linq;
using System.Reflection;

namespace Rop.Dapper.ContribEx
{
    /// <summary>
    /// Immutable class with Key Description for another Class
    /// </summary>
    public class KeyDescription
    {
        public string TableName { get; }
        public string KeyName { get; }
        public bool IsAutoKey { get; }
        public PropertyInfo KeyProp { get; }
        public bool KeyTypeIsString { get; }
        public bool IsForeignTable { get; }
        public string ForeignDatabaseName { get; }
        public string GetUse()
        {
            return (IsForeignTable) ? $"USE {ForeignDatabaseName}; " : "";
        }
        internal KeyDescription(string tableName, string keyName, bool isAutoKey, PropertyInfo keyProp)
        {
            TableName = tableName;
            KeyName = keyName;
            IsAutoKey = isAutoKey;
            KeyProp = keyProp;
            KeyTypeIsString = Type.GetTypeCode(KeyProp.PropertyType) == TypeCode.String;
            IsForeignTable = IsAForeignTable(tableName,out var fdbn);
            ForeignDatabaseName = fdbn;
        }
        internal KeyDescription(string foreignDatabase, string tableName, string keyName, bool isAutoKey, PropertyInfo keyProp)
        {
            TableName = tableName;
            KeyName = keyName;
            IsAutoKey = isAutoKey;
            KeyProp = keyProp;
            KeyTypeIsString = Type.GetTypeCode(KeyProp.PropertyType) == TypeCode.String;
            IsForeignTable = true;
            ForeignDatabaseName = foreignDatabase;
        }

        public static bool IsAForeignTable(string tablename, out string foreigndatabase)
        {
            foreigndatabase = "";
            var res = tablename.Count(c => c == '.') >= 2;
            foreigndatabase = (res) ? tablename.Split('.').FirstOrDefault() : "";
            return res;
        }
    }
}