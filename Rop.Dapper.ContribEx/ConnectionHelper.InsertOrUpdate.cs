using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
        public static int InsertOrUpdateAutoKey<T>(this IDbConnection conn, T item, IDbTransaction tr = null) where T : class
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var key = (int) (DapperHelperExtend.GetKeyValue(item));
            if (key <= 0)
            {
                key = (int)conn.Insert(item, tr);
            }
            else
            {
                conn.Update(item, tr);
            }
            return key;
        }
        public static void InsertOrUpdate<T>(this IDbConnection conn, T item, IDbTransaction tr = null) where T : class
        {
            var res = conn.Update(item, tr);
            if (!res) conn.Insert(item, tr);
        }
        public static int UpdateIdValues<TA, T>(this IDbConnection conn, IEnumerable<(dynamic id, T value)> values, string field, IDbTransaction tr = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"UPDATE {kd.TableName} SET {field}=@value WHERE {kd.KeyName}=@id";

            var lstdyn = new List<object>();
            foreach (var dbIdValue in values)
            {
                lstdyn.Add(new { id = dbIdValue.id, value = dbIdValue.value });
            }
            return conn.Execute(sql, lstdyn, tr);
        }
        public static bool UpdateIdValue<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"UPDATE {kd.TableName} SET {field}=@value WHERE {kd.KeyName}=@id";
            var r=conn.Execute(sql, new { id = value.id, value = value.value }, tr);
            return r == 1;
        }

        // Async

        public static async Task<int> InsertOrUpdateAutoKeyAsync<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var key = (int)(DapperHelperExtend.GetKeyValue(item));
            if (key <= 0)
            {
                key =(int)(await conn.InsertAsync(item, tr,timeout));
            }
            else
            {
                await conn.UpdateAsync(item, tr,timeout);
            }
            return key;
        }
        public static async Task InsertOrUpdateAsync<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            var res = await conn.UpdateAsync(item, tr,timeout);
            if (!res) await conn.InsertAsync(item, tr,timeout);
        }
        public static async Task<int> UpdateIdValuesAsync<TA, T>(this IDbConnection conn, IEnumerable<(dynamic id, T value)> values, string field, IDbTransaction tr = null,int? timeout=null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"UPDATE {kd.TableName} SET {field}=@value WHERE {kd.KeyName}=@id";

            var lstdyn = new List<object>();
            foreach (var dbIdValue in values)
            {
                lstdyn.Add(new { id = dbIdValue.id, value = dbIdValue.value });
            }
            return await conn.ExecuteAsync(sql, lstdyn, tr,timeout);
        }
        public static async Task<bool> UpdateIdValueAsync<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null,int? timeout=null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"UPDATE {kd.TableName} SET {field}=@value WHERE {kd.KeyName}=@id";
            var r=await conn.ExecuteAsync(sql, new { id = value.id, value = value.value }, tr,timeout);
            return r == 1;
        }

    }
}
