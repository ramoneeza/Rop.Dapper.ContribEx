using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
        public static int InsertOrUpdate<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var objkey = DapperHelperExtend.GetKeyValue(item);
            if (kd.IsAutoKey)
            {
                var key = (int)objkey;
                if (key <= 0)
                {
                    key = (int) conn.Insert(item, tr,timeout);
                }
                else
                {
                    conn.Update(item, tr,timeout);
                }
                return key;
            }
            else
            {
                if (objkey is int i)
                {
                    var res = conn.Update(item, tr, timeout);
                    if (!res) conn.Insert(item, tr, timeout);
                    return i;
                }
                else
                {
                    i = 1;
                    var res = conn.Update(item, tr, timeout);
                    if (!res) conn.Insert(item, tr, timeout);
                    return i; // i=1 == sucessful 
                }
            }
        }
        public static bool UpdateIdValue<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null, int? timeout = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(TA));
            var sql = $"UPDATE {kd.TableName} SET {field}=@value WHERE {kd.KeyName}=@id";
            var r=conn.Execute(sql, new { id = value.id, value = value.value }, tr,timeout);
            return r == 1;
        }
        public static bool UpdateIdValue<TA, T>(this IDbConnection conn, dynamic id, T value, string field, IDbTransaction tr = null, int? timeout = null)
        {
            return UpdateIdValue<TA, T>(conn, (id, value), field, tr, timeout);
        }
        
        // Async

        public static async Task<int> InsertOrUpdateAsync<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=null) where T : class
        {
            return await Task.Run(() => conn.InsertOrUpdate(item, tr, timeout));

        }
        public static async Task<bool> UpdateIdValueAsync<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null,int? timeout=null)
        {
            return await Task.Run(() => UpdateIdValue<TA, T>(conn, value, field, tr, timeout));
        }
        public static async Task<bool> UpdateIdValueAsync<TA, T>(this IDbConnection conn, dynamic id, T value, string field, IDbTransaction tr = null, int? timeout = null)
        {
            return await Task.Run(() => UpdateIdValue<TA, T>(conn,id, value, field, tr, timeout));
        }

    }
}
