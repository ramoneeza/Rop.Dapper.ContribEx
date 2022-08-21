using Dapper;
using Dapper.Contrib.Extensions;
using Rop.Dapper.ContribEx;
using System.Data;
using System.Linq;
using xUnit.Rop.Dapper.ContribEx.Data;
using Xunit;

namespace xUnit.Rop.Dapper.ContribEx
{
    public class ConnectionHelperTest
    {

        // We can change TestSuite to other DB Engines
        private static readonly ITestSuite TestSuite = new SQLiteTestSuite();
        private IDbConnection GetOpenConnection()
        {
            var connection = GetConnection();
            connection.Open();
            TestSuite.CreateMemoryTables(connection);
            return connection;
        }
        private IDbConnection GetConnection()
        {
            return TestSuite.GetConnection();
        }
        private Car _createCar(int id = 1) => new Car()
        {
            Id = id,
            Name = "Ford",
            Computed = "America"
        };
        private ObjectX _createObjectX(string id = "MyKey") => new ObjectX()
        {
            ObjectXId = id,
            Name = "Ford"
        };

        private static (Car, Car, Car) _createCars()
        {
            var c1 = new Car()
            {
                Id = 1,
                Name = "Ford",
                Computed = "America"
            };
            var c2 = new Car()
            {
                Id = 2,
                Name = "Ford",
                Computed = "Europe"
            };
            var c3 = new Car()
            {
                Id = 3,
                Name = "Renault",
                Computed = "Europe"
            };
            return (c1, c2, c3);
        }
        private static (User, User, User) _createUsers()
        {
            var c1 = new User()
            {
                Id = 1,
                Name = "Pepe",
                Age = 20
            };
            var c2 = new User()
            {
                Id = 2,
                Name = "Pepe",
                Age = 31
            };
            var c3 = new User()
            {
                Id = 3,
                Name = "John",
                Age = 40
            };

            return (c1, c2, c3);
        }

        private void _insertCar(IDbConnection conn, int key = 1)
        {
            var item = _createCar(key);
            conn.Insert(item);
        }

        private void _insertObjectX(IDbConnection conn, string key = "MyKey")
        {
            var item = _createObjectX(key);
            conn.Insert(item);

        }

        [Fact]
        public void DeleteByKeyTest()
        {
            using (var conn = GetOpenConnection())
            {
                _insertCar(conn);
                _insertObjectX(conn);
                var r1 = conn.DeleteByKey<Car>(1);
                Assert.True(r1);
                var r2 = conn.DeleteByKey<ObjectX>("MyKey");
                Assert.True(r2);
                // Already deleted
                r1 = conn.DeleteByKey<Car>(1);
                Assert.False(r1);
                r2 = conn.DeleteByKey<ObjectX>("MyKey");
                Assert.False(r2);
            }
        }

        [Fact]
        public void DeleteTest()
        {

            using (var conn = GetOpenConnection())
            {
                _insertCar(conn);
                _insertObjectX(conn);
                using (var tr = conn.BeginTransaction())
                {
                    var r1 = conn.DeleteByKey<Car>(1,tr);
                    Assert.True(r1);
                    var r2 = conn.DeleteByKey<ObjectX>("MyKey",tr);
                    Assert.True(r2);
                    tr.Commit();
                }

                using (var tr2 = conn.BeginTransaction())
                {
                    // Already deleted
                    var r1 = conn.DeleteByKey<Car>(1,tr2);
                    Assert.False(r1);
                    var r2 = conn.DeleteByKey<ObjectX>("MyKey",tr2);
                    Assert.False(r2);
                    tr2.Commit();
                }
            }

        }

        [Fact]
        public void GetSomeTest()
        {

            using (var conn = GetOpenConnection())
            {
                for (var f = 1; f < 9; f++) _insertCar(conn, f);
                for (var f = 'A'; f < 'F'; f++) _insertObjectX(conn, f.ToString());
                var k1 = new[] { 4, 5, 6 };
                var lst1 = conn.GetSome<Car>(k1);
                var k2 = new[] { "B", "C", "D" };
                var lst2 = conn.GetSome<ObjectX>(k2);
                Assert.Equal(k1, lst1.Select(i => i.Id));
                Assert.Equal(k2, lst2.Select(i => i.ObjectXId));
            }

        }

        [Fact]
        public void GetWhereTest()
        {

            using (var conn = GetOpenConnection())
            {
                
                    _deleteCars(conn);
                    var (c1, c2, c3) = _createCars();
                    conn.Insert(c1);
                    conn.Insert(c2);
                    conn.Insert(c3);
                    
                    var lst1 = conn.GetWhere<Car>("Name=@name", new { name = "Ford" }).Select(c => c.Id).ToArray();
                    Assert.Equal(new[] { 1, 2 }, lst1);
                
            }

        }

        private void _deleteCars(IDbConnection conn)
        {
            conn.Execute($"DELETE from {DapperHelperExtend.GetTableName(typeof(Car))}");
        }

        [Fact]
        public void QueryIdValue()
        {

            using (var conn = GetOpenConnection())
            {
                var (c1, c2, c3) = _createUsers();
                conn.Insert(c1);
                conn.Insert(c2);
                conn.Insert(c3);
                var lst1 = conn.QueryIdValue<User, int>("Age", "Name=@name", new { name = "Pepe" })
                    .OrderBy(c => c.id).Select(c => ((int)c.id, c.value));
                Assert.Equal(new (int, int)[] { (c1.Id, c1.Age), (c2.Id, c2.Age) }, lst1);
            }

        }
        [Fact]
        public void GetIdValues()
        {

            using (var conn = GetOpenConnection())
            {
               
                    var (c1, c2, c3) = _createUsers();
                    conn.Insert(c1);
                    conn.Insert(c2);
                    conn.Insert(c3);
                   
                    var lst1 = conn.GetIdValues<User, int>(new[] { 1, 2 }, "Age").OrderBy(c => c.id)
                        .Select(c => ((int)c.id, c.value)).ToArray();
                    Assert.Equal(new (int, int)[] { (c1.Id, c1.Age), (c2.Id, c2.Age) }, lst1);
                
            }

        }

        [Fact]
        public void InsertOrUpdateAutoKeyText()
        {

            var c1 = new User() { Id = 0, Name = "Ford", Age = 20 };
            var c2 = new User() { Id = 0, Name = "Seat", Age = 25 };
            using (var conn = GetOpenConnection())
            {
               

                    conn.Execute("DELETE FROM users");
                    var k1 = conn.InsertOrUpdate(c1);
                    DapperHelperExtend.SetKeyValue(c1, k1);
                    var cr1 = conn.Get<User>(k1);
                    Assert.Equal(c1.Id, cr1.Id);
                    Assert.Equal(c1.Name, cr1.Name);
                    Assert.Equal(c1.Age, cr1.Age);
                    DapperHelperExtend.SetKeyValue(c2, k1);
                    conn.InsertOrUpdate(c2);
                    
                    var cr2 = conn.Get<User>(1);
                    Assert.Equal(c2.Id, cr2.Id);
                    Assert.Equal(c2.Name, cr2.Name);
                    Assert.Equal(c2.Age, cr2.Age);
                
            }

        }
        [Fact]
        public void UpdateIdValueText()
        {

            var c1 = new User() { Id = 0, Name = "Ford", Age = 20 };
            using (var conn = GetOpenConnection())
            {
                conn.Execute("DELETE FROM users");
                var k1 = (int)conn.Insert(c1);
                DapperHelperExtend.SetKeyValue(c1, k1);
                conn.UpdateIdValue<User, string>((k1, "Seat"), "Name");
                var cr1 = conn.Get<User>(k1);
                Assert.Equal(k1, cr1.Id);
                Assert.Equal("Seat", cr1.Name);
                Assert.Equal(c1.Age, cr1.Age);
            }

        }

    }
}
