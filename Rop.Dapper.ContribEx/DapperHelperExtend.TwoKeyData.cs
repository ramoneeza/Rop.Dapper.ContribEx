using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Dapper.ContribEx
{
    public static partial class DapperHelperExtend
{
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, TwoKeyDescription> TwoKeyDescriptions = new ConcurrentDictionary<RuntimeTypeHandle, TwoKeyDescription>();

    /// <summary>
    /// Get Single Key for class of type t
    /// </summary>
    /// <param name="t">Type of class</param>
    /// <returns>Property and autokey flag</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static (PropertyInfo prop1key,PropertyInfo prop2key) GetDoubleKey(Type t)
    {
        if (t == null) throw new ArgumentNullException(nameof(t));
        var explicitKeys = ExplicitKeyPropertiesCache(t);
        var keyCount = explicitKeys.Count;
        if (keyCount != 2) throw new InvalidOperationException($"Type {t} has not double key");
        return (explicitKeys[0],explicitKeys[1]);
    }
    /// <summary>
    /// Get Key Description for class of type t
    /// </summary>
    /// <param name="t">Type of class</param>
    /// <returns>KeyDescription</returns>
    public static TwoKeyDescription GetTwoKeyDescription(Type t)
    {
        if (TwoKeyDescriptions.TryGetValue(t.TypeHandle, out var kd)) return kd;
        var (prop1key,prop2key) = GetDoubleKey(t);
        var key1name = prop1key.Name;
        var key2name = prop2key.Name;
        var tname = GetTableName(t);
        var fdb = GetForeignDatabaseName(t);
        kd = fdb is null ? new TwoKeyDescription(tname, key1name,key2name, prop1key,prop2key) : new TwoKeyDescription(fdb,tname, key1name,key2name, prop1key,prop2key);
        TwoKeyDescriptions[t.TypeHandle]= kd;
        return kd;
    }

    public static TwoKeyDescription GetTwoKeyDescription<T>() where T:class
    {
        return GetTwoKeyDescription(typeof(T));
    }

    /// <summary>
    /// Get Key Value for item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">Item</param>
    /// <returns>Item's Key</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static (object,object) GetTwoKeyValue<T>(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        var kd = GetTwoKeyDescription(typeof(T));
        return (kd.Key1Prop.GetValue(item),kd.Key2Prop.GetValue(item));
    }
    /// <summary>
    /// Get Key Description and Key Value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <returns>Key description and Key value</returns>
    public static (TwoKeyDescription keydescription, object key1,object key2) GetTwoKeyDescriptionAndValue<T>(T item)
    {
        var kd = GetTwoKeyDescription(typeof(T));
        var v = GetTwoKeyValue(item);
        return (kd, v.Item1,v.Item2);
    }

}
}
