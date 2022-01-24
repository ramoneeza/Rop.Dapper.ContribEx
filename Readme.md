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
string GetTableName(Type type);
List<PropertyInfo> TypePropertiesCache(Type type);
List<PropertyInfo> ComputedPropertiesCache(Type type);
ISqlAdapter GetFormatter(IDbConnection connection);
IEnumerable<string> GetColumnNames(IEnumerable<PropertyInfo> props);
string SelectGetCache(Type type);
string SelectGetAllCache(Type type);
string GetDeleteByKeyCache(Type type);
```

The full list of helpers for class/table/key information are:

```csharp
(PropertyInfo propkey, bool isautokey) GetSingleKey(Type t);
KeyDescription GetKeyDescription(Type t);
object GetKeyValue<T>(T item);
SetKeyValue<T>(T item, object value);
(KeyDescription keydescription, object value) GetKeyDescriptionAndValue<T>(T item);
```
The full list of helpers to format list of keys are:

```csharp
string GetIdList(IEnumerable<int> ids);
string GetIdList(IEnumerable<string> ids);
static string GetIdListDyn(IEnumerable ids);
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
 bool DeleteByKey<T>(this IDbConnection conn, object id, IDbTransaction tr = null);
 bool Delete<T>(this IDbConnection conn, IDbTransaction tr, int key);
 bool Delete<T>(this IDbConnection conn, IDbTransaction tr, string key);
 Task<bool> DeleteByKeyAsync<T>(this IDbConnection conn, object id, IDbTransaction tr = null, int? commandTimeout = null);
 Task<bool> DeleteAsync<T>(this IDbConnection conn, IDbTransaction tr, int key,int? commandTimeout=null);
 Task<bool> DeleteAsync<T>(this IDbConnection conn, IDbTransaction tr, string key,int? commandTimeout=null);
 ```

`GetSome` methods
-------

```csharp
List<T> GetSome<T>(this IDbConnection conn, IEnumerable<string> ids, IDbTransaction tr = null) where T : class;
List<T> GetSome<T>(this IDbConnection conn, IEnumerable<int> ids, IDbTransaction tr = null) where T : class;
List<T> GetSomeDyn<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null) where T : class;
List<T> GetWhere<T>(this IDbConnection conn, string where, object param, IDbTransaction tr = null) where T : class;
IEnumerable<(dynamic id, T value)> QueryIdValue<TA, T>(this IDbConnection conn, string field, string where, object param, IDbTransaction tr = null);
IEnumerable<(dynamic id, A valueA, B valueB)> QueryIdValue<TA, A, B>(this IDbConnection conn, string fieldA, string fieldB, string where, object param, IDbTransaction tr = null);
IEnumerable<(dynamic id, T value)> GetIdValues<TA, T>(this IDbConnection conn, IEnumerable ids, string field, IDbTransaction tr = null);
IEnumerable<(dynamic id, A valueA, B valueB)> GetIdValues<TA, A, B>(this IDbConnection conn, IEnumerable ids, string fieldA, string fieldB, IDbTransaction tr = null);
Task<List<T>> IntGetSomeAsync<T>(this IDbConnection conn, string lst, IDbTransaction tr = null,int? timeout=null) where T : class;
Task<List<T>> GetSomeAsync<T>(this IDbConnection conn, IEnumerable<string> ids, IDbTransaction tr = null,int? timeout=null) where T : class;
Task<List<T>> GetSomeAsync<T>(this IDbConnection conn, IEnumerable<int> ids, IDbTransaction tr = null,int? timeout=null) where T : class;
Task<List<T>> GetSomeDynAsync<T>(this IDbConnection conn, IEnumerable ids, IDbTransaction tr = null,int? timeout=null) where T : class;
Task<List<T>> GetWhereAsync<T>(this IDbConnection conn, string where, object param, IDbTransaction tr = null,int? timeout=null) where T : class;
Task<IEnumerable<(dynamic id, T value)>> QueryIdValueAsync<TA, T>(this IDbConnection conn, string field, string where, object param, IDbTransaction tr = null,int? timeout=null);
Task<IEnumerable<(dynamic id, A valueA, B valueB)>> QueryIdValueAsync<TA, A, B>(this IDbConnection conn, string fieldA, string fieldB, string where, object param, IDbTransaction tr = null,int? timeout=null);
Task<List<(dynamic id, T value)>> GetIdValuesAsync<TA, T>(this IDbConnection conn, IEnumerable ids, string field, IDbTransaction tr = null,int? timeout=null);
Task<List<(dynamic id, A valueA, B valueB)>> GetIdValuesAsync<TA, A, B>(this IDbConnection conn, IEnumerable ids, string fieldA, string fieldB, IDbTransaction tr = null,int? timeout=null);
```

`Insert or Update` methods
-------

```csharp
int InsertOrUpdateAutoKey<T>(this IDbConnection conn, T item, IDbTransaction tr = null) where T : class;
void InsertOrUpdate<T>(this IDbConnection conn, T item, IDbTransaction tr = null) where T : class;
int UpdateIdValues<TA, T>(this IDbConnection conn, IEnumerable<(dynamic id, T value)> values, string field, IDbTransaction tr = null);
bool UpdateIdValue<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null);
Task<int> InsertOrUpdateAutoKeyAsync<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=0) where T : class;
Task InsertOrUpdateAsync<T>(this IDbConnection conn, T item, IDbTransaction tr = null,int? timeout=0) where T : class;
Task<int> UpdateIdValuesAsync<TA, T>(this IDbConnection conn, IEnumerable<(dynamic id, T value)> values, string field, IDbTransaction tr = null,int? timeout=null);
Task<bool> UpdateIdValueAsync<TA, T>(this IDbConnection conn, (dynamic id, T value) value, string field, IDbTransaction tr = null,int? timeout=null);
```

`Complex` methods
-------

```csharp
 List<T> QueryJoin<T, M>(this IDbConnection conn, string query, object param, Action<T, M> join, IDbTransaction tr = null);
 ```

 ------
 (C)2022 Ramón Ordiales Plaza
