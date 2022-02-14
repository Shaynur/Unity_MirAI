using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.MirAI.DB {

    public class DbContext : IDisposable {

        private static readonly string _connectionString = GetDatabaseConnectionString();
        private readonly SqliteConnection _connection;

        public DbNode Nodes { get; set; }
        public DbProgram Programs { get; set; }
        public DbLink Links { get; set; }

        public bool IsOpen => _connection.State == System.Data.ConnectionState.Open;

        public DbContext() {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            ExecuteCommand("PRAGMA foreign_keys = ON;");
            Nodes = new DbNode("Nodes", _connection);
            Programs = new DbProgram("Programs", _connection);
            Links = new DbLink("Links", _connection);
        }

        private static string GetDatabaseConnectionString() {
            string dbNamePrefix = @"URI=file:";
            string dbFileName = @"MirAI.db";
            string dbFullPath = Path.Combine(Application.dataPath, @"DB", dbFileName);
            string dbConnectionString = dbNamePrefix + dbFullPath;
            return dbConnectionString;
        }

        public void Dispose() {
            _connection.Close();
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
    }
}