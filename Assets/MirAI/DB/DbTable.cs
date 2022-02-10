using System;
using System.Collections.Generic;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {

    public abstract class DbTable<T> where T : IAiModelElement {

        public string TableName { get; set; }
        private readonly SqliteConnection _connection;

        public abstract T CreateByData(IDataRecord data);
        public abstract string GetInsertCommandSuffix(T t);
        public abstract string GetDeleteCommandSuffix(T t);
        public abstract string GetUpdateCommandSuffix(T t);

        public DbTable(string tableName, SqliteConnection connection) {
            TableName = tableName;
            _connection = connection;
        }

        public List<T> ToList() {
            try {
                var entities = new List<T>();
                using var command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM " + TableName + ";";
                using IDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var entity = CreateByData(reader);
                    entities.Add(entity);
                }
                reader.Close();
                return entities;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert " + typeof(T).Name + " to list error.", ex);
            }
        }

        public T GetById(int id) {
            try {
                T entity = default;
                using var command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM " + TableName + @" WHERE Id='" + id.ToString() + "';";
                using IDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    entity = CreateByData(reader);
                }
                reader.Close();
                return entity;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error when try GetById " + typeof(T).Name + ". Id= " + id, ex);
            }
        }

        public void Add(T entity) {
            var commandPrefix = "INSERT INTO " + TableName;
            var commandValues = GetInsertCommandSuffix(entity);
            ExecuteCommand(commandPrefix + commandValues);
            entity.Id = GetLastId();
        }

        public void Update(T entity) {
            var commandPrefix = "UPDATE " + TableName;
            var commandValues = GetUpdateCommandSuffix(entity);
            ExecuteCommand(commandPrefix + commandValues);
        }

        public void Remove(T entity) {
            var commandPrefix = "DELETE FROM " + TableName;
            var commandValues = GetDeleteCommandSuffix(entity);
            ExecuteCommand(commandPrefix + commandValues);
        }

        public void Remove(int id) {
            var commandText = "DELETE FROM " + TableName + " WHERE Id = '" + id + "';";
            ExecuteCommand(commandText);
        }

        public int GetLastId() {
            try {
                using var command = _connection.CreateCommand();
                command.CommandText = "SELECT * FROM " + TableName + " WHERE rowid=last_insert_rowid();";
                using IDataReader reader = command.ExecuteReader();
                int id = -1;
                if (reader.Read())
                    id = reader.GetInt32(0);
                reader.Close();
                return id;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error when try GetLastId " + typeof(T).Name + ".", ex);
            }
        }

        private void ExecuteCommand(string commandText) {
            try {
                using var command = _connection.CreateCommand();
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error execute some NonQuery command for " + typeof(T).Name + ".", ex);
            }
        }
    }
}