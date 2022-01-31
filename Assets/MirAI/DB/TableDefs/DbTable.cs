using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB.TableDefs {

    public class DbTable<T> where T : IDbEntity, new() {

        public string TableName { get; set; }
        private List<T> Entities { get; set; } = null!;
        private readonly SqliteConnection _connection;

        public DbTable(string tableName, SqliteConnection connection) {
            TableName = tableName;
            _connection = connection;
        }

        public virtual List<T> ToList() {
            FillEntitiesList();
            return Entities;
        }

        private void FillEntitiesList() {
            Entities = new List<T>();
            using var command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM " + TableName + ";";
            using IDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var entity = new T();
                entity.SetData((IDataRecord)reader);
                Entities.Add(entity);
            }
            reader.Close();
        }

        public virtual T GetById(int id) {
            T entity = default;

            using var command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM " + TableName + @" WHERE Id='" + id.ToString() + "';";

            using IDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                if (entity == null) entity = new T();
                entity.SetData((IDataRecord)reader);
            }
            reader.Close();

            return entity;
        }

        public virtual void Add(T entity) {
            var commandPrefix = "INSERT INTO " + TableName;
            var commandValues = entity.GetInsertCommandSuffix();
            ExecuteCommand(commandPrefix + commandValues);
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
            var commandText = "DELETE FROM " + TableName + " WHERE Id = '" + id + "';"; ;
            ExecuteCommand(commandText);
        }

        private void ExecuteCommand(string commandText) {
            using var command = _connection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}