using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Rop.Dapper.ContribEx
{
    /// <summary>
    /// Extends Dapper to obtain internal data
    /// </summary>
    public static partial class DapperHelperExtend
    {
        /// <summary>
        /// Access to ExplicitKey Properties Cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of properties</returns>
        public static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)=>Invoke<List<PropertyInfo>>(ExplicitKeyPropertiesCacheInfo, type);
        /// <summary>
        /// Access to Key Properties Cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of properties</returns>
        public static List<PropertyInfo> KeyPropertiesCache(Type type)=>Invoke<List<PropertyInfo>>(KeyPropertiesCacheInfo, type);
        /// <summary>
        /// Get table name for a class
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Table name</returns>
        public static string GetTableName(Type type) => Invoke<string>(GetTableNameInfo, type);
        /// <summary>
        /// Access to Type Properties Cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of properties</returns>
        public static List<PropertyInfo> TypePropertiesCache(Type type)=>Invoke<List<PropertyInfo>>(TypePropertiesCacheInfo, type);
        /// <summary>
        /// Access to Computed Properties Cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of properties</returns>
        public static List<PropertyInfo> ComputedPropertiesCache(Type type) => Invoke<List<PropertyInfo>>(ComputedPropertiesCacheInfo, type);
        /// <summary>
        /// Access to connection formatter
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>ISqlAdaptar</returns>
        public static ISqlAdapter GetFormatter(IDbConnection connection)=>Invoke<ISqlAdapter>(GetFormatterInfo, connection);
        /// <summary>
        /// Convert propertyinfo to names
        /// </summary>
        /// <param name="props"></param>
        /// <returns>List of columns names</returns>
        public static IEnumerable<string> GetColumnNames(IEnumerable<PropertyInfo> props)=>props.Select(p => p.Name);
        /// <summary>
        /// Access to Cache for Get query
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Sql string</returns>
        public static string SelectGetCache(Type type)
        {
            if (!GetQueries.TryGetValue(type.TypeHandle, out string sql))
            {
                var (key,_) = GetSingleKey(type);
                var name = GetTableName(type);
                sql = $"select * from {name} where {key.Name} = @id";
                GetQueries[type.TypeHandle] = sql;
            }
            return sql;
        }
        /// <summary>
        /// Access to Cache for GetAll query
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Sql String</returns>
        public static string SelectGetAllCache(Type type)
        {
            var lstgeneric = typeof(List<>);
            var listoft = lstgeneric.MakeGenericType(type);
            if (!GetQueries.TryGetValue(listoft.TypeHandle, out string sql))
            {
                var name = GetTableName(type);
                sql = $"select * from {name}";
                GetQueries[listoft.TypeHandle] = sql;
            }
            return sql;
        }
        /// <summary>
        /// Access to Cache for Delete query
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Sql String</returns>
        public static string DeleteByKeyCache(Type type)
        {
            if (!DeleteByKeyDic.TryGetValue(type.TypeHandle, out string sql))
            {
                var (key, _) = GetSingleKey(type);
                var name = GetTableName(type);
                sql = $"DELETE FROM {name} WHERE {key.Name} = @id";
                DeleteByKeyDic[type.TypeHandle] = sql;
            }
            return sql;
        }
        public static string GetForeignDatabaseName(Type type)
        {
            if (ForeignDatabase.TryGetValue(type.TypeHandle, out string name)) return name;
            var foreigndatabaseAttrName = type.GetCustomAttribute<ForeignDatabaseAttribute>(false)?.Name
                                          ?? (type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == nameof(ForeignDatabaseAttribute)) as dynamic)?.Name;

            if (foreigndatabaseAttrName != null) name = foreigndatabaseAttrName;
            ForeignDatabase[type.TypeHandle] = name;
            return name;
        }
        public static string SelectGetAllSlimCache(Type type)
        {
            if (SelectSlimDic.TryGetValue(type.TypeHandle, out string partialSelect)) return partialSelect;

            var name = GetTableName(type);
            var allProperties = TypePropertiesCache(type);
            var proplst = string.Join(", ", allProperties.Select(p => p.Name));
            partialSelect = $"select {proplst} from {name}";
            SelectSlimDic[type.TypeHandle] = partialSelect;
            return partialSelect;
        }

        public static string SelectGetSlimCache(Type type)
        {
            if (GetSlimDic.TryGetValue(type.TypeHandle, out var queryslim)) return queryslim;
            var select = SelectGetAllSlimCache(type);
            var (key, _) = GetSingleKey(type);
            queryslim = $"{select} where {key.Name} = @id";
            GetSlimDic[type.TypeHandle] = queryslim;
            return queryslim;
        }





        /// <summary>
        /// Get Expression's member name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">Expression</param>
        /// <returns>Member name</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetMemberName<T>(this Expression<T> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression m:
                    return m.Member.Name;
                case UnaryExpression u:
                    if (u.Operand is MemberExpression m2) return m2.Member.Name;
                    throw new NotImplementedException(expression.GetType().ToString());
                default:
                    throw new NotImplementedException(expression.GetType().ToString());
            }
        }
        /// <summary>
        /// Convert Ienumerable of int keys to string
        /// </summary>
        /// <param name="ids">IEnumerable of keys</param>
        /// <returns>string</returns>
        public static string GetIdList(IEnumerable<int> ids)=>string.Join(",", ids);
        
        /// <summary>
        /// Convert Ienumerable of string keys to string
        /// </summary>
        /// <param name="ids">IEnumerable of keys</param>
        /// <returns>string</returns>
        public static string GetIdList(IEnumerable<string> ids)=>string.Join(",", ids.Select(i=>$"'{i}'"));

        /// <summary>
        /// Convert Ienumerable of dynamic keys to string
        /// </summary>
        /// <param name="ids">IEnumerable of keys</param>
        /// <returns>string</returns>
        public static string GetIdListDyn(IEnumerable ids)
        {
            var idsobj = ids.Cast<object>().ToArray();
            if (idsobj.Length == 0) return "";
            var id0 = idsobj[0];
            return id0 is string ? GetIdList(idsobj.Cast<string>()) : GetIdList(idsobj.Cast<int>());
        }
    }
}