using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
        public static bool DeleteByKey<T>(this IDbConnection conn, dynamic id, IDbTransaction tr = null, int? commandTimeout = null)
        {
            var sql = DapperHelperExtend.GetDeleteByKeyCache(typeof(T));
            var dynParams = new DynamicParameters();
            dynParams.Add("@id", id);
            var n = conn.Execute(sql, dynParams, tr, commandTimeout);
            return n > 0;
        }

        public static bool Delete<T>(this IDbConnection conn, IDbTransaction tr, dynamic key, int? commandTimeout = null)
        {
            return DeleteByKey<T>(conn, key, tr, commandTimeout);
        }

        // Async 
        public static async Task<bool> DeleteByKeyAsync<T>(this IDbConnection conn, dynamic id, IDbTransaction tr = null, int? commandTimeout = null)
        {
            var sql = DapperHelperExtend.GetDeleteByKeyCache(typeof(T));
            var dynParams = new DynamicParameters();
            dynParams.Add("@id", id);
            var n = await conn.ExecuteAsync(sql, dynParams, tr, commandTimeout);
            return n > 0;
        }

        public static Task<bool> DeleteAsync<T>(this IDbConnection conn, IDbTransaction tr, dynamic id, int? commandTimeout = null)
        {
            return DeleteByKeyAsync<T>(conn, id, tr, commandTimeout);
        }
    }
}
