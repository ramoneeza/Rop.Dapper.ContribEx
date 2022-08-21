# Rop.Dapper.ContribEx

Features
--------

Rop.Dapper.ContribEx includes a DapperHelperExtend class to access to hidden information about
classes and their attributes.

Rop.Dapper.ContribEx adds more helper methods for inserting, getting,
updating and deleting records.

## DapperHelperExtend

The full list of helpers for hidden information are:

```csharp
List<PropertyInfo> ExplicitKeyPropertiesCache(Type type);
List<PropertyInfo> KeyPropertiesCache(Type type);
List<PropertyInfo> TypePropertiesCache(Type type);
List<PropertyInfo> ComputedPropertiesCache(Type type);
ISqlAdapter GetFormatter(IDbConnection connection);
IEnumerable<string> GetColumnNames(IEnumerable<PropertyInfo> props);
string GetTableName(Type type);
string GetForeignDatabaseName(Type type);
string SelectGetCache(Type type);
string SelectGetAllCache(Type type);
string SelectGetSlimCache(Type type);
string SelectGetAllSlimCache(Type type);
string DeleteByKeyCache(Type type);

```

The full list of helpers for class/table/key information are:

```csharp
(PropertyInfo propkey, bool isautokey) GetSingleKey(Type t);
KeyDescription GetKeyDescription(Type t);
KeyDescription GetKeyDescription<T>();
object GetKeyValue<T>(T item);
SetKeyValue<T>(T item, object value);
(KeyDescription keydescription, object value) GetKeyDescriptionAndValue<T>(T item);

```
The full list of helpers to format list of keys are:

```csharp
string GetIdList(IEnumerable<int> ids);
string GetIdList(IEnumerable<string> ids);
string GetIdListDyn(IEnumerable ids);
```

Miscellaneous helpers:

```csharp
string GetMemberName<T>(this Expression<T> expression);
```

## ConnectionHelper

The full list of extension methods in ConnectionHelper are:

`Delete` methods
-------

```csharp
 bool DeleteByKey<T>(this IDbConnection conn, dynamic id, IDbTransaction tr = null, int? commandTimeout = null);
 bool Delete<T>(this IDbConnection conn, IDbTransaction tr, dynamic key, int? commandTimeout = null);
 Task<bool> DeleteByKeyAsync<T>(this IDbConnection conn, dynamic id, IDbTransaction tr = null, int? commandTimeout = null);
 Task<bool> DeleteAsync<T>(this IDbConnection conn, IDbTransaction tr, dynamic key,int? commandTimeout=null);
 ```

`GetSlim` methods
------

```csharp
T GetSlim<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null);
IEnumerable<T> GetAllSlim<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null);
List<T> GetSomeSlim<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null);
List<T> GetWhereSlim<T>(this IDbConnection conn, string where, object param=null, IDbTransaction tr = null);
Task<T> GetSlimAsync<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null);
Task<IEnumerable<T>> GetAllSlimAsync<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null);
```

`GetSome` methods
-------

```csharp
List<T> GetSome<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null, int? commandTimeout = null);
List<T> GetWhere<T>(this IDbConnection conn, string where, object param=null, IDbTransaction tr = null, int? commandTimeout = null);
IEnumerable<(dynamic id, T value)> QueryIdValue<TA, T>(this IDbConnection conn, string field, string where, object param=null, IDbTransaction tr = null, int? commandTimeout = null);
IEnumerable<(dynamic id, A valueA, B valueB)> QueryIdValue<TA, A, B>(this IDbConnection conn, string fieldA, string fieldB, string where, object param=null, IDbTransaction tr = null, int? commandTimeout = null);
IEnumerable<(dynamic id, T value)> GetIdValues<TA, T>(this IDbConnection conn, IEnumerable ids, string field, IDbTransaction tr = null,int? commandtimeout=null);
IEnumerable<(dynamic id, A valueA, B valueB)> GetIdValues<TA, A, B>(this IDbConnection conn, IEnumerable ids, string fieldA, string fieldB, IDbTransaction tr = null,int? commandtimeout=null);
Task<List<T>> GetSomeAsync<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null,int? timeout=null);
Task<List<T>> GetWhereAsync<T>(this IDbConnection conn, string where, object param=null, IDbTransaction tr = null,int? timeout=null);
Task<IEnumerable<(dynamic id, T value)>> QueryIdValueAsync<TA, T>(this IDbConnection conn, string field, string where, object param=null, IDbTransaction tr = null,int? timeout=null);
Task<IEnumerable<(dynamic id, A valueA, B valueB)>> QueryIdValueAsync<TA, A, B>(this IDbConnection conn, string fieldA, string fieldB, string where, object param=null, IDbTransaction tr = null,int? timeout=null);
Task<List<(dynamic id, T value)>> GetIdValuesAsync<TA, T>(this IDbConnection conn, IEnumerable ids, string field, IDbTransaction tr = null,int? timeout=null);
Task<List<(dynamic id, A valueA, B valueB)>> GetIdValuesAsync<TA, A, B>(this IDbConnection conn, IEnumerable ids, string fieldA, string fieldB, IDbTransaction tr = null,int? timeout=null);      
```

`Insert or Update` methods
-------

```csharp
int InsertOrUpdate<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=null);
bool UpdateIdValue<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null, int? timeout = null);
bool UpdateIdValue<TA, T>(this IDbConnection conn, dynamic id, T value, string field, IDbTransaction tr = null, int? timeout = null);
Task<int> InsertOrUpdateAsync<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=null);
Task<bool> UpdateIdValueAsync<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null,int? timeout=null);
Task<bool> UpdateIdValueAsync<TA, T>(this IDbConnection conn, dynamic id, T value, string field, IDbTransaction tr = null, int? timeout = null);
```

`Complex` methods
-------

```csharp
 List<T> QueryJoin<T, M>(this IDbConnection conn, string query, object param, Action<T, M> join, IDbTransaction tr = null);
 ```

 ------
 (C)2022 Ramón Ordiales Plaza
