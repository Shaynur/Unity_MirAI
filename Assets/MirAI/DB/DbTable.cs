using System;
using System.Collections.Generic;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {

    public abstract class DbTable<T> where T : IHaveId {

        protected string TableName { get; set; }
        public abstract T GetFromReader(IDataRecord data);
        public abstract SqliteCommand GetCreateTableCommand();
        public abstract SqliteCommand GetDeleteCommand(T t);
        public abstract SqliteCommand GetInsertCommand(T t);
        public abstract SqliteCommand GetUpdateCommand(T t);

        protected readonly SqliteConnection _connection;

        public DbTable(string tableName, SqliteConnection connection) {
            TableName = tableName;
            _connection = connection;
            CreateTableIfNotExist();
        }

        public List<T> ToList() {
            try {
                var entities = new List<T>();
                using var command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM " + TableName + ";";
                using IDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var entity = GetFromReader(reader);
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
                    entity = GetFromReader(reader);
                }
                reader.Close();
                return entity;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error when try GetById " + typeof(T).Name + ". Id= " + id, ex);
            }
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

        public void CreateTableIfNotExist() {
            var command = GetCreateTableCommand();
            ExecuteCommand(command);
        }

        public int Add(T entity) {
            var command = GetInsertCommand(entity);
            var retval = ExecuteCommand(command);
            entity.Id = GetLastId();
            return retval;
        }

        public int Update(T entity) {
            var command = GetUpdateCommand(entity);
            return ExecuteCommand(command);
        }

        public int Remove(T entity) {
            var command = GetDeleteCommand(entity);
            return ExecuteCommand(command);
        }

        private int ExecuteCommand(SqliteCommand command) {
            int retval;
            try {
                retval = command.ExecuteNonQuery();
            }
            catch (Exception ex) {
                throw new DbMirAiException("Error execute some NonQuery command for " + typeof(T).Name + ".", ex);
            }
            finally {
                command.Dispose();
            }
            return retval;
        }
    }
}