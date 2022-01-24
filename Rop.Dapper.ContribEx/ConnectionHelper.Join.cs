using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Rop.Dapper.ContribEx
{
    public static partial class ConnectionHelper
    {
        public static List<T> QueryJoin<T, M>(this IDbConnection conn, string query, object param, Action<T, M> join, IDbTransaction tr = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(T));
            if (kd.KeyTypeIsString)
                return _queryJoin<T, M, string>(conn, query, param, join, tr);
            else
                return _queryJoin<T, M, int>(conn, query, param, join, tr);
        }
        private static List<T> _queryJoin<T, M, K>(IDbConnection conn, string query, object param, Action<T, M> join, IDbTransaction tr = null)
        {
            var kd = DapperHelperExtend.GetKeyDescription(typeof(T));
            var kd2 = DapperHelperExtend.GetKeyDescription(typeof(M));
            var dic = new Dictionary<K, T>();
            var res1 = conn.Query<T, M, T>(query, map: (t, m) =>
            {
                var key = (K)DapperHelperExtend.GetKeyValue(t);
                if (!dic.TryGetValue(key, out var v))
                {
                    v = t;
                    dic[key] = t;
                }
                @join(v, m);
                return v;
            }, param: param, splitOn: kd2.KeyName, transaction: tr).ToList();
            return dic.Values.ToList();
        }
    }
}
