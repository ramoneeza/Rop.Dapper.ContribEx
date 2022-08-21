using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.Contrib.Extensions.SqlMapperExtensions;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
       public static T GetSlim<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var sql = DapperHelperExtend.SelectGetSlimCache(type);
            var dynParams = new DynamicParameters();
            dynParams.Add("@id", id);
            T obj = connection.Query<T>(sql, dynParams, transaction, commandTimeout: commandTimeout).FirstOrDefault();
            return obj;
        }
        public static IEnumerable<T> GetAllSlim<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var sql = DapperHelperExtend.SelectGetAllSlimCache(type);
            var result = connection.Query<T>(sql,null,transaction,true,commandTimeout);
            return result;
        }
        private static List<T> IntGetSomeSlim<T>(this IDbConnection conn, string lst, IDbTransaction tr = null) where T : class
        {
            var keyd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var sql = DapperHelperExtend.SelectGetAllSlimCache(typeof(T));
            return conn.Query<T>($"{sql} WHERE {keyd.KeyName} IN ({lst})", null, tr).ToList();
        }
        public static List<T> GetSomeSlim<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null) where T : class
        {
            var lst = DapperHelperExtend.GetIdListDyn(ids);
            return IntGetSomeSlim<T>(conn, lst, tr);
        }

        public static List<T> GetWhereSlim<T>(this IDbConnection conn, string where, object param=null, IDbTransaction tr = null) where T : class
        {
            var sql = DapperHelperExtend.SelectGetAllSlimCache(typeof(T));
            return conn.Query<T>($"{sql} WHERE {@where}",param, tr).ToList();
        }

        // Async
        public static async Task<T> GetSlimAsync<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var sql = DapperHelperExtend.SelectGetSlimCache(type);
            var dynParams = new DynamicParameters();
            dynParams.Add("@id", id);
            return await connection.QueryFirstOrDefaultAsync<T>(sql, dynParams,transaction,commandTimeout).ConfigureAwait(false);
        }
        public static async Task<IEnumerable<T>> GetAllSlimAsync<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var sql = DapperHelperExtend.SelectGetAllSlimCache(type);
            var result = await connection.QueryAsync<T>(sql,null,transaction,commandTimeout).ConfigureAwait(false);
            return result;
        }
    }
}
