using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.MirAI.DB {

    public class DbContext : IDisposable {

        public DbProgram Programs { get; set; }
        public DbNode Nodes { get; set; }
        public DbLink Links { get; set; }

        private static string _connectionString = GetDatabaseConnectionString("MirAI.db");
        private SqliteConnection _connection;

        public bool IsOpen => _connection.State == System.Data.ConnectionState.Open;

        public DbContext() {
            OpenDb();
        }

        public DbContext(string dbFileName) {
            _connectionString = GetDatabaseConnectionString(dbFileName);
            OpenDb();
        }

        private void OpenDb() {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            ExecuteCommand("PRAGMA foreign_keys = ON;");
            Programs = new DbProgram("Programs", _connection);
            Nodes = new DbNode("Nodes", _connection);
            Links = new DbLink("Links", _connection);
        }

        private static string GetDatabaseConnectionString(string dbFileName) {
            string dbNamePrefix = "URI=file:";
            string dbFullPath = Path.Combine(Application.dataPath, "DB", dbFileName);
            string dbConnectionString = dbNamePrefix + dbFullPath;
            return dbConnectionString;
        }

        private void ExecuteCommand(string commandText) {
            try {
                using var command = _connection.CreateCommand();
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error execute some NonQuery command in DbContect.", ex);
            }
        }

        public void Dispose() {
            _connection.Close();
        }
    }
}