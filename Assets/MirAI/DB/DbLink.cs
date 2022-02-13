using System;
using System.Data;
using Assets.MirAI.Models;
using Mono.Data.Sqlite;

namespace Assets.MirAI.DB {
    public class DbLink : DbTable<Link> {

        public DbLink(string tableName, SqliteConnection connection) : base(tableName, connection) {
        }

        public override string GetDeleteCommandSuffix(Link link) {
            return " WHERE FromId = '" + link.FromId + "' AND ToId = '" + link.ToId + "';";
        }

        public override string GetInsertCommandSuffix(Link link) {
            return " (FromId, ToId) VALUES ('" + link.FromId + "', '" + link.ToId + "');";
        }

        public override string GetUpdateCommandSuffix(Link link) {
            return ";";
        }

        public override Link CreateByData(IDataRecord data) {
            try {
                Link link = new Link(data.GetInt32(0), data.GetInt32(1));
                return link;
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }
    }
}