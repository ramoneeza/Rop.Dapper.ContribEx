using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
        public static bool DeleteByKey<T>(this IDbConnection conn, object id, IDbTransaction tr = null)
        {
            var sql = DapperHelperExtend.GetDeleteByKeyCache(typeof(T));
            var dynParams = new DynamicParameters();
            dynParams.Add("@id", id);
            var n = conn.Execute(sql, dynParams, tr);
            return n > 0;
        }

        public static bool Delete<T>(this IDbConnection conn, IDbTransaction tr, int key)
        {
            return DeleteByKey<T>(conn, key, tr);
        }
        public static bool Delete<T>(this IDbConnection conn, IDbTransaction tr, string key)
        {
            return DeleteByKey<T>(conn, key, tr);
        }

        // Async 

        public static async Task<bool> DeleteByKeyAsync<T>(this IDbConnection conn, object id, IDbTransaction tr = null, int? commandTimeout = null)
        {
            var sql = DapperHelperExtend.GetDeleteByKeyCache(typeof(T));
            var dynParams = new DynamicParameters();
            dynParams.Add("@id", id);
            var n = await conn.ExecuteAsync(sql, dynParams, tr,commandTimeout);
            return n > 0;
        }

        public static async Task<bool> DeleteAsync<T>(this IDbConnection conn, IDbTransaction tr, int key,int? commandTimeout=null)
        {
            return await DeleteByKeyAsync<T>(conn, key, tr,commandTimeout);
        }
        public static async Task<bool> DeleteAsync<T>(this IDbConnection conn, IDbTransaction tr, string key,int? commandTimeout=null)
        {
            return await DeleteByKeyAsync<T>(conn, key, tr,commandTimeout);
        }


    }
}
