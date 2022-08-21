using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Rop.Dapper.ContribEx
{
    public static partial class DapperHelperExtend
    {
        private static readonly MethodInfo ExplicitKeyPropertiesCacheInfo;

        private static readonly MethodInfo KeyPropertiesCacheInfo;

        private static readonly MethodInfo GetTableNameInfo;

        private static readonly MethodInfo TypePropertiesCacheInfo;

        private static readonly MethodInfo ComputedPropertiesCacheInfo;

        private static readonly MethodInfo GetFormatterInfo;

        private static readonly FieldInfo GetQueriesInfo;

        private static T Invoke<T>(MethodInfo method, params object[] args)
        {
            Debug.Assert(method != null, nameof(method) + " != null");
            return (T)method.Invoke(null, args);
        }

        private static ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries => (ConcurrentDictionary<RuntimeTypeHandle,string>)GetQueriesInfo.GetValue(null);

        private static ConcurrentDictionary<RuntimeTypeHandle, string> ForeignDatabase = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static ConcurrentDictionary<RuntimeTypeHandle, string> SelectSlimDic = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static ConcurrentDictionary<RuntimeTypeHandle, string> GetSlimDic = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static ConcurrentDictionary<RuntimeTypeHandle, string> DeleteByKeyDic = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        
        static DapperHelperExtend()
        {
            ExplicitKeyPropertiesCacheInfo = GetInfo("ExplicitKeyPropertiesCache");
            KeyPropertiesCacheInfo = GetInfo("KeyPropertiesCache");
            GetTableNameInfo = GetInfo("GetTableName");
            TypePropertiesCacheInfo = GetInfo("TypePropertiesCache");
            ComputedPropertiesCacheInfo = GetInfo("ComputedPropertiesCache");
            GetFormatterInfo = GetInfo("GetFormatter");
            GetQueriesInfo= typeof(SqlMapperExtensions).GetField("GetQueries", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException($"Invalid field GetMethod in static constructor");
            MethodInfo GetInfo(string name) => typeof(SqlMapperExtensions).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException($"Invalid method {name} in static constructor");
        }
        
        

    }
}
