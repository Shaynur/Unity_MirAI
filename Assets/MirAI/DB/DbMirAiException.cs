using System;

namespace Assets.MirAI.DB {
    public class DbMirAiException : Exception {
        public DbMirAiException(string message) : base(message) { }
        public DbMirAiException(string message, Exception innerException) : base(message, innerException) { }
    }
}