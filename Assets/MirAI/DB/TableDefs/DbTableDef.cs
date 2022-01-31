using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.MirAI.DB.TableDefs {

    public class DbTableDef<T> where T : IDbTable, new() {

        public string TableName { get; set; }
        private static readonly string ConnectionString = GetDatabaseConnectionString();

        public DbTableDef(string tableName) {
            TableName = tableName;
        }

        public virtual List<T> GetAllRecords() {
            var entityList = new List<T>();
            using (var connection = new SqliteConnection(ConnectionString)) {
                connection.Open();
                using (var command = connection.CreateCommand()) {
                    command.CommandText = @"SELECT * FROM " + TableName + ";";
                    using (IDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var entity = new T();
                            entity.SetData((IDataRecord)reader);
                            entityList.Add(entity);
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return entityList;
        }

        private static string GetDatabaseConnectionString() {
            string dbNamePrefix = @"URI=file:";
            string dbFileName = @"MirAI.db";
            string dbFullPath = Path.Combine(Application.dataPath, @"DB", dbFileName);
            string dbConnectionString = dbNamePrefix + dbFullPath;
            return dbConnectionString;
        }
    }
}