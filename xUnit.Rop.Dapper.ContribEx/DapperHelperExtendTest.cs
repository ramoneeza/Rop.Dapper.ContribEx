using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Rop.Dapper.ContribEx;
using Xunit;
using xUnit.Rop.Dapper.ContribEx.Data;

namespace xUnit.Rop.Dapper.ContribEx
{
    public partial class DapperHelperExtendTest
    {
        // We can change TestSuite to other DB Engines
        private static readonly ITestSuite TestSuite =new SQLiteTestSuite();
        private IDbConnection GetOpenConnection()
        {
            var connection = GetConnection();
            connection.Open();
            return connection;
        }
        private IDbConnection GetConnection()
        {
            return TestSuite.GetConnection();
        }
        private Car _createCar()=> new Car()
        {
            Id = 1,
            Name = "Ford",
            Computed = "America"
        };
        private ObjectX _createObjectX()=>new ObjectX()
        {
            ObjectXId = "MyKey",
            Name = "Ford"
        };

        [Fact]
        public void ExplicitKeyPropertiesCacheTest()
        {
            // No explicitKey
            var explicitkeys = DapperHelperExtend.ExplicitKeyPropertiesCache(typeof(Car));
            Assert.Empty(explicitkeys);
            // Single explicitKey
            explicitkeys=DapperHelperExtend.ExplicitKeyPropertiesCache(typeof(GenericType<int>));
            Assert.Single(explicitkeys);
            // Check Twice to check cache
            explicitkeys=DapperHelperExtend.ExplicitKeyPropertiesCache(typeof(GenericType<int>));
            Assert.Single(explicitkeys);
            // Double explicit Key
            explicitkeys=DapperHelperExtend.ExplicitKeyPropertiesCache(typeof(DoubleKey1));
            Assert.Equal(2,explicitkeys.Count());
        }
        [Fact]
        public void KeyPropertiesCacheTest()
        {
            //Single Key
            var autokeys = DapperHelperExtend.KeyPropertiesCache(typeof(Car));
            Assert.Single(autokeys);
            // Check Twice to check cache
            autokeys=DapperHelperExtend.KeyPropertiesCache(typeof(Car));
            Assert.Single(autokeys);
            // No autoKey
            autokeys=DapperHelperExtend.KeyPropertiesCache(typeof(GenericType<int>));
            Assert.Empty(autokeys);
            // Double Key
            autokeys=DapperHelperExtend.KeyPropertiesCache(typeof(DoubleKey2));
            Assert.Equal(2,autokeys.Count());
        }

        [Fact]
        public void GetTableNameTest()
        {
            var tablename = DapperHelperExtend.GetTableName(typeof(Car));
            Assert.Equal("Automobiles",tablename);
        }
        [Fact]
        public void TypePropertiesCacheTest()
        {
            var properties = DapperHelperExtend.TypePropertiesCache(typeof(Car));
            Assert.Equal(new []{ "Id","Name","Computed" },properties.Select(p=>p.Name));
        }
        [Fact]
        public void ComputedPropertiesCacheTest()
        {
            var properties = DapperHelperExtend.ComputedPropertiesCache(typeof(Car));
            Assert.Equal(new []{ "Computed" },properties.Select(p=>p.Name));
            properties = DapperHelperExtend.ComputedPropertiesCache(typeof(ObjectX));
            Assert.Empty(properties);
        }
        [Fact]
        public void GetFormatterTest()
        {
            var formatter = DapperHelperExtend.GetFormatter(GetConnection());
            Assert.NotNull(formatter);
        }
        [Fact]
        public void SelectGetCacheTest()
        {
            var select = DapperHelperExtend.SelectGetCache(typeof(Car));
            Assert.Equal("select * from Automobiles where Id = @id",select);
        }
        [Fact]
        public void SelectGetAllCacheTest()
        {
            var select = DapperHelperExtend.SelectGetAllCache(typeof(Car));
            Assert.Equal("select * from Automobiles",select);
        }
        [Fact]
        public void GetDeleteByKeyCacheTest()
        {
            var select = DapperHelperExtend.DeleteByKeyCache(typeof(Car));
            Assert.Equal("DELETE FROM Automobiles WHERE Id = @id",select);
        }
        // Class / Table / Key info

        [Fact]
        public void GetSingleKeyTest()
        {
            var (propkey,isautokey) = DapperHelperExtend.GetSingleKey(typeof(Car));
            Assert.Equal("Id",propkey.Name);
            Assert.True(isautokey);
            // Explicit
            (propkey,isautokey) = DapperHelperExtend.GetSingleKey(typeof(GenericType<int>));
            Assert.Equal("Id",propkey.Name);
            Assert.False(isautokey);
        }
        [Fact]
        public void GetKeyDescriptionTest1()
        {
            var keyDescription = DapperHelperExtend.GetKeyDescription(typeof(Car));
            Assert.Equal("Automobiles",keyDescription.TableName);
            Assert.Equal("Id",keyDescription.KeyName);
            Assert.NotNull(keyDescription.KeyProp);
            Assert.True(keyDescription.IsAutoKey);
            Assert.False(keyDescription.KeyTypeIsString);
        }
        [Fact]
        public void GetKeyDescriptionTest2()
        {
            var keyDescription = DapperHelperExtend.GetKeyDescription(typeof(ObjectX));
            Assert.Equal("ObjectX",keyDescription.TableName);
            Assert.Equal("ObjectXId",keyDescription.KeyName);
            Assert.NotNull(keyDescription.KeyProp);
            Assert.False(keyDescription.IsAutoKey);
            Assert.True(keyDescription.KeyTypeIsString);
        }
        [Fact]
        public void GetKeyDescriptionAndValueTest1()
        {
            var item = _createCar();
            var (keyDescription,value) = DapperHelperExtend.GetKeyDescriptionAndValue(item);
            Assert.Equal("Automobiles",keyDescription.TableName);
            Assert.Equal("Id",keyDescription.KeyName);
            Assert.True(keyDescription.IsAutoKey);
            Assert.False(keyDescription.KeyTypeIsString);
            Assert.Equal(1,value);
        }
        [Fact]
        public void GetKeyDescriptionAndValueTest2()
        {
            var item = _createObjectX();
            var (keyDescription,value) = DapperHelperExtend.GetKeyDescriptionAndValue(item);
            Assert.Equal("ObjectX",keyDescription.TableName);
            Assert.Equal("ObjectXId",keyDescription.KeyName);
            Assert.False(keyDescription.IsAutoKey);
            Assert.True(keyDescription.KeyTypeIsString);
            Assert.Equal("MyKey",value);
        }
        [Fact]
        public void GetKeyValueTest()
        {
            var item1 = _createCar();
            var item2 = _createObjectX();
            var value1 = DapperHelperExtend.GetKeyValue(item1);
            var value2 = DapperHelperExtend.GetKeyValue(item2);
            Assert.Equal(1,value1);
            Assert.Equal("MyKey",value2);
        }
        [Fact]
        public void SetKeyValueTest()
        {
            var item1 = _createCar();
            var item2 = _createObjectX();
            DapperHelperExtend.SetKeyValue(item1,2);
            DapperHelperExtend.SetKeyValue(item2,"OtherKey");
            Assert.Equal(2,item1.Id);
            Assert.Equal("OtherKey",item2.ObjectXId);
        }

        // IdLists
        [Fact]
        public void GetIdListTest()
        {
            var lst1 = DapperHelperExtend.GetIdList(new []{1,2,3,4,5,6});
            var lst2 = DapperHelperExtend.GetIdList(new []{"A","B","C","D"});
            var lst3 = DapperHelperExtend.GetIdListDyn(new object[]{"A","B"});
            var lst4 = DapperHelperExtend.GetIdListDyn(new object[]{1,2});
            Assert.Equal("1,2,3,4,5,6",lst1);
            Assert.Equal("'A','B','C','D'",lst2);
            Assert.Equal("'A','B'",lst3);
            Assert.Equal("1,2",lst4);
        }
        // ExternalDatabase tests
        [Fact]
        public void GetExternalDatabaseName()
        {
            var name1 = DapperHelperExtend.GetForeignDatabaseName(typeof(ExtWithAttTable));
            var name2 = DapperHelperExtend.GetForeignDatabaseName(typeof(ExtWithAttTable2));
            var name3 = DapperHelperExtend.GetForeignDatabaseName(typeof(ExtWithKey1));
            var name4 = DapperHelperExtend.GetForeignDatabaseName(typeof(ExtWithKey2));
            var desired = "Intranet";
            Assert.Equal(desired,name1);
            Assert.Equal(desired,name2);
            Assert.Equal(desired,name3);
            Assert.Equal(desired,name4);
        }

    }
}
