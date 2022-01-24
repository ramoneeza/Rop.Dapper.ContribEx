using System;
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
        internal KeyDescription(string tableName, string keyName, bool isAutoKey, PropertyInfo keyProp)
        {
            TableName = tableName;
            KeyName = keyName;
            IsAutoKey = isAutoKey;
            KeyProp = keyProp;
            KeyTypeIsString = Type.GetTypeCode(KeyProp.PropertyType) == TypeCode.String;
        }
    }
}