using System.Data;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;

namespace xUnit.Rop.Dapper.ContribEx
{
    public interface ITestSuite
    {
        IDbConnection GetConnection();
        void CreateMemoryTables(IDbConnection connection);
    }

    public class SQLiteTestSuite:ITestSuite
    {
        private const string FileName = "Test.DB.sqlite";
        public static string ConnectionString => $"Data Source=:memory:;Mode=ReadWriteCreate;Pooling=True;";
        public IDbConnection GetConnection() => new SqliteConnection(ConnectionString);
        public static bool InMemory => true;
        static SQLiteTestSuite()
        {
            // Only if file db
            //if (File.Exists(FileName))
            //{
            //    File.Delete(FileName);
            //}
            //using (var connection = new SqliteConnection(ConnectionString))
            //{
            //    connection.Open();
            //    CreateTables(connection);
            //}
        }

        public static void CreateTables(IDbConnection connection)
        {
            connection.Execute("CREATE TABLE Stuff (TheId integer primary key autoincrement not null, Name nvarchar(100) not null, Created DateTime null) ");
            connection.Execute("CREATE TABLE People (Id integer primary key autoincrement not null, Name nvarchar(100) not null) ");
            connection.Execute("CREATE TABLE Users (Id integer primary key autoincrement not null, Name nvarchar(100) not null, Age int not null) ");
            connection.Execute("CREATE TABLE Automobiles (Id integer primary key autoincrement not null, Name nvarchar(100) not null) ");
            connection.Execute("CREATE TABLE Results (Id integer primary key autoincrement not null, Name nvarchar(100) not null, [Order] int not null) ");
            connection.Execute("CREATE TABLE ObjectX (ObjectXId nvarchar(100) not null, Name nvarchar(100) not null) ");
            connection.Execute("CREATE TABLE ObjectY (ObjectYId integer not null, Name nvarchar(100) not null) ");
            connection.Execute("CREATE TABLE ObjectZ (Id integer not null, Name nvarchar(100) not null) ");
            connection.Execute("CREATE TABLE GenericType (Id nvarchar(100) not null, Name nvarchar(100) not null) ");
            connection.Execute("CREATE TABLE NullableDates (Id integer primary key autoincrement not null, DateValue DateTime) ");
        }

        public void CreateMemoryTables(IDbConnection connection)
        {
            if (InMemory) CreateTables(connection);
        }
    }
}