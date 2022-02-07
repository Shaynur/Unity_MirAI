using System;
using System.Data;
using Assets.MirAI.Models;

namespace Assets.MirAI.DB {
    public class DbLink : IDbRoutines {

        private readonly Link _link;

        public DbLink(Link link) {
            _link = link;
        }

        public string GetDeleteCommandSuffix() {
            return " WHERE FromId = '" + _link.FromId + "' AND ToId = '" + _link.ToId + "';";
        }

        public string GetInsertCommandSuffix() {
            return " (FromId, ToId) VALUES ('" + _link.FromId + "', '" + _link.ToId + "');";
        }

        public string GetUpdateCommandSuffix() {
            return ";";
        }

        public void SetData(IDataRecord data) {
            try {
                _link.FromId = data.GetInt32(0);
                _link.ToId = data.GetInt32(1);
            }
            catch (Exception ex) {
                throw new DbMirAiException("Convert IDataRecord to Program error.", ex);
            }
        }
    }
}