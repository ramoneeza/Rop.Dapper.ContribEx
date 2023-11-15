using System;
using System.Linq;
using System.Reflection;

namespace Rop.Dapper.ContribEx
{
    /// <summary>
    /// Immutable class with Key Description for another Class
    /// </summary>
    public class TwoKeyDescription
    {
        public string TableName { get; }
        public string Key1Name { get; }
        public string Key2Name { get; }
        public PropertyInfo Key1Prop { get; }
        public PropertyInfo Key2Prop { get; }
        public bool IsForeignTable { get; }
        public string ForeignDatabaseName { get; }
        public string GetUse()
        {
            return (IsForeignTable) ? $"USE {ForeignDatabaseName}; " : "";
        }
        internal TwoKeyDescription(string tableName, string key1Name,string key2Name, PropertyInfo key1Prop,PropertyInfo key2prop)
        {
            TableName = tableName;
            Key1Name = key1Name;
            Key2Name = key2Name;
            Key1Prop = key1Prop;
            Key2Prop = key2prop;
            IsForeignTable = IsAForeignTable(tableName,out var fdbn);
            ForeignDatabaseName = fdbn;
        }
        internal TwoKeyDescription(string foreignDatabase, string tableName, string key1Name,string key2Name, PropertyInfo key1Prop,PropertyInfo key2Prop)
        {
            TableName = tableName;
            Key1Name = key1Name;
            Key2Name = key2Name;
            Key1Prop = key1Prop;
            Key2Prop = key2Prop;
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