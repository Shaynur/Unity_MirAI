using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;


namespace Assets.MirAI.DB {

    public class DbTable<T> where T : IDbEntity, new() {

        public string TableName { get; set; }
        private readonly SqliteConnection _connection;

        public DbTable(string tableName, SqliteConnection connection) {
            TableName = tableName;
            _connection = connection;
        }

        public virtual List<T> ToList() {
            try {
                var entities = new List<T>();
                using var command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM " + TableName + ";";
                using IDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var entity = new T();
                    entity.SetData(reader);
                    entities.Add(entity);
                }
                reader.Close();
                return entities;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert " + typeof(T).Name + " to list error.", ex);
            }
        }

        public virtual T GetById(int id) {
            try {
                T entity = default;
                using var command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM " + TableName + @" WHERE Id='" + id.ToString() + "';";
                using IDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    if (entity == null) entity = new T();
                    entity.SetData(reader);
                }
                reader.Close();
                return entity;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error when try GetById " + typeof(T).Name + ". Id= " + id, ex);
            }
        }

        public virtual void Add(T entity) {
            var commandPrefix = "INSERT INTO " + TableName;
            var commandValues = entity.GetInsertCommandSuffix();
            ExecuteCommand(commandPrefix + commandValues);
            entity.Id = GetLastId();
        }

        public virtual void Update(T entity) {
            var commandPrefix = "UPDATE " + TableName;
            var commandValues = entity.GetUpdateCommandSuffix();
            ExecuteCommand(commandPrefix + commandValues);
        }

        public virtual void Remove(T entity) {
            var commandPrefix = "DELETE FROM " + TableName;
            var commandValues = entity.GetDeleteCommandSuffix();
            ExecuteCommand(commandPrefix + commandValues);
        }

        public virtual void Remove(int id) {
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