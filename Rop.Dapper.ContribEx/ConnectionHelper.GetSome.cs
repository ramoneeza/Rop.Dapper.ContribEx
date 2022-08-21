using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
        private static List<T> IntGetSome<T>(this IDbConnection conn, string lst, IDbTransaction tr = null,int? commandTimeout=null) where T : class
        {
            var keyd = DapperHelperExtend.GetKeyDescription(typeof(T));
            return conn.Query<T>($"SELECT * FROM {keyd.TableName} WHERE {keyd.KeyName} IN ({lst})", null, tr,true,commandTimeout).ToList();
        }

        public static List<T> GetSome<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null, int? commandTimeout = null) where T : class
        {
            var lst = DapperHelperExtend.GetIdListDyn(ids);
            return IntGetSome<T>(conn, lst, tr,commandTimeout);
        }

        public static List<T> GetWhere<T>(this IDbConnection conn, string where, object param=null, IDbTransaction tr = null, int? commandTimeout = null) where T : class
        {
            var keyd = DapperHelperExtend.GetKeyDescription(typeof(T));
            return conn.Query<T>($"SELECT * FROM {keyd.TableName} WHERE {@where}",param, tr,true,commandTimeout).ToList();
        }
        public static IEnumerable<(dynamic id, T value)> QueryIdValue<TA, T>(this IDbConnection conn, string field, string where, object param=null, IDbTransaction tr = null, int? commandTimeout = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"SELECT {kd.KeyName} as Id, {field} as Value FROM {kd.TableName} WHERE {@where}";
            var q = conn.Query<dynamic>(sql, param, tr,true,commandTimeout);
            return q.Select(qq => (qq.Id, (T)qq.Value));
        }
        public static IEnumerable<(dynamic id, A valueA, B valueB)> QueryIdValue<TA, A, B>(this IDbConnection conn, string fieldA, string fieldB, string where, object param=null, IDbTransaction tr = null, int? commandTimeout = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"SELECT {kd.KeyName} as Id, {fieldA} as ValueA,{fieldB} as ValueB FROM {kd.TableName} WHERE {@where}";
            var q = conn.Query<dynamic>(sql, param, tr, true, commandTimeout);
            return q.Select(qq => (qq.Id, (A)qq.ValueA, (B)qq.ValueB));
        }
        
        public static IEnumerable<(dynamic id, T value)> GetIdValues<TA, T>(this IDbConnection conn, IEnumerable ids, string field, IDbTransaction tr = null,int? commandtimeout=null)
        {
            var lstids = DapperHelperExtend.GetIdListDyn(ids);
            if (lstids != "")
            {
                var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
                var sql = $"SELECT {kd.KeyName} as Id, {field} as Value FROM {kd.TableName} WHERE {kd.KeyName} IN ({lstids})";
                var q = conn.Query<dynamic>(sql, null, tr,true,commandtimeout);
                foreach (var qq in q)
                {
                    yield return (qq.Id, (T)qq.Value);
                }
            }
        }
        public static IEnumerable<(dynamic id, A valueA, B valueB)> GetIdValues<TA, A, B>(this IDbConnection conn, IEnumerable ids, string fieldA, string fieldB, IDbTransaction tr = null,int? commandtimeout=null)
        {
            var lstids = DapperHelperExtend.GetIdListDyn(ids);
            if (lstids != "")
            {
                var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
                var sql = $"SELECT {kd.KeyName} as Id, {fieldA} as ValueA,{fieldB} as ValueB FROM {kd.TableName} WHERE {kd.KeyName} IN ({lstids})";
                var q = conn.Query<dynamic>(sql, null, tr).ToList();
                foreach (var qq in q)
                {
                    yield return (qq.Id, qq.ValueA, qq.ValueB);
                }
            }
        }

        // Async

        private static async Task<List<T>> IntGetSomeAsync<T>(this IDbConnection conn, string lst, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            var keyd = DapperHelperExtend.GetKeyDescription(typeof(T));
            return (await conn.QueryAsync<T>($"SELECT * FROM {keyd.TableName} WHERE {keyd.KeyName} IN ({lst})", null, tr,timeout)).ToList();
        }

        public static async Task<List<T>> GetSomeAsync<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            var keyd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var lst = DapperHelperExtend.GetIdListDyn(ids);
            var q= await conn.QueryAsync<T>($"SELECT * FROM {keyd.TableName} WHERE {keyd.KeyName} IN ({lst})", null, tr, timeout);
            return q.ToList();
        }
        
        public static async Task<List<T>> GetWhereAsync<T>(this IDbConnection conn, string where, object param=null, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            var keyd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var q = await conn.QueryAsync<T>($"SELECT * FROM {keyd.TableName} WHERE {@where}", param, tr, timeout);
            return q.ToList();
        }

        public static async Task<IEnumerable<(dynamic id, T value)>> QueryIdValueAsync<TA, T>(this IDbConnection conn, string field, string where, object param=null, IDbTransaction tr = null,int? timeout=null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"SELECT {kd.KeyName} as Id, {field} as Value FROM {kd.TableName} WHERE {@where}";
            var q = await conn.QueryAsync<dynamic>(sql, param, tr,timeout);
            return q.Select(qq => (qq.Id, (T)qq.Value));
        }
        public static async Task<IEnumerable<(dynamic id, A valueA, B valueB)>> QueryIdValueAsync<TA, A, B>(this IDbConnection conn, string fieldA, string fieldB, string where, object param=null, IDbTransaction tr = null,int? timeout=null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"SELECT {kd.KeyName} as Id, {fieldA} as ValueA,{fieldB} as ValueB FROM {kd.TableName} WHERE {@where}";
            var q = await conn.QueryAsync<dynamic>(sql, param, tr,timeout);
            return q.Select(qq => (qq.Id, (A)qq.ValueA, (B)qq.ValueB));
        }
        public static async Task<List<(dynamic id, T value)>> GetIdValuesAsync<TA, T>(this IDbConnection conn, IEnumerable ids, string field, IDbTransaction tr = null,int? timeout=null)
        {
            var lstids = DapperHelperExtend.GetIdListDyn(ids);
            if (lstids != "")
            {
                var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
                var sql = $"SELECT {kd.KeyName} as Id, {field} as Value FROM {kd.TableName} WHERE {kd.KeyName} IN ({lstids})";
                var q = await conn.QueryAsync<dynamic>(sql, null, tr,timeout);
                return q.Select(qq => (qq.Id, (T)qq.Value)).ToList();
            }
            else
            {
                return new List<(dynamic id, T value)>();
            }
        }
        public static async Task<List<(dynamic id, A valueA, B valueB)>> GetIdValuesAsync<TA, A, B>(this IDbConnection conn, IEnumerable ids, string fieldA, string fieldB, IDbTransaction tr = null,int? timeout=null)
        {
            var lstids = DapperHelperExtend.GetIdListDyn(ids);
            if (lstids != "")
            {
                var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
                var sql = $"SELECT {kd.KeyName} as Id, {fieldA} as ValueA,{fieldB} as ValueB FROM {kd.TableName} WHERE {kd.KeyName} IN ({lstids})";
                var q = await conn.QueryAsync<dynamic>(sql, null, tr,timeout);
                return q.Select(qq=>(qq.Id, (A)qq.ValueA, (B)qq.ValueB)).ToList();
            }
            else
            {
                return new List<(dynamic id, A valueA, B valueB)>();
            }
        }

    }
}
