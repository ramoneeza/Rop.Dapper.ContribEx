using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Dapper.ContribEx
{
    public static partial class DapperHelperExtend
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, KeyDescription> KeyDescriptions = new ConcurrentDictionary<RuntimeTypeHandle, KeyDescription>();

        /// <summary>
        /// Get Single Key for class of type t
        /// </summary>
        /// <param name="t">Type of class</param>
        /// <returns>Property and autokey flag</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static (PropertyInfo propkey, bool isautokey) GetSingleKey(Type t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            var keys = KeyPropertiesCache(t);
            var explicitKeys = ExplicitKeyPropertiesCache(t);
            var keyCount = keys.Count + explicitKeys.Count;
            if (keyCount != 1) throw new InvalidOperationException($"Type {t} has not single key");
            return (keys.Count > 0) ? (keys[0], true) : (explicitKeys[0], false);
        }
        /// <summary>
        /// Get Key Description for class of type t
        /// </summary>
        /// <param name="t">Type of class</param>
        /// <returns>KeyDescription</returns>
        public static KeyDescription GetKeyDescription(Type t)
        {
            if (KeyDescriptions.TryGetValue(t.TypeHandle, out var kd)) return kd;
            var (propkey, isautokey) = GetSingleKey(t);
            var keyname = propkey.Name;
            var tname = GetTableName(t);
            kd = new KeyDescription(tname, keyname, isautokey, propkey);
            KeyDescriptions[t.TypeHandle]= kd;
            return kd;
        }
        /// <summary>
        /// Get Key Value for item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">Item</param>
        /// <returns>Item's Key</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetKeyValue<T>(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var kd = GetKeyDescription(typeof(T));
            return kd.KeyProp.GetValue(item);
        }
        /// <summary>
        /// Set Key for Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetKeyValue<T>(T item, object value)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var kd = GetSingleKey(typeof(T));
            kd.propkey.SetValue(item, value);
        }
        /// <summary>
        /// Get Key Description and Key Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>Key description and Key value</returns>
        public static (KeyDescription keydescription, object value) GetKeyDescriptionAndValue<T>(T item)
        {
            var kd = GetKeyDescription(typeof(T));
            var v = GetKeyValue(item);
            return (kd, v);
        }

    }
}
