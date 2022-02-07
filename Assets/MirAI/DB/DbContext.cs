using System;
using System.IO;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.MirAI.DB {

    public class DbContext : IDisposable {

        private static readonly string _connectionString = GetDatabaseConnectionString();
        private readonly SqliteConnection _connection;

        public DbTable<Node> Nodes { get; set; }
        public DbTable<Program> Programs { get; set; }
        public DbTable<Link> Links { get; set; }

        public bool IsOpen => _connection.State == System.Data.ConnectionState.Open;

        public DbContext() {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            Nodes = new DbTable<Node>("Nodes", _connection);
            Programs = new DbTable<Program>("Programs", _connection);
            Links = new DbTable<Link>("Links", _connection);
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
    }
}