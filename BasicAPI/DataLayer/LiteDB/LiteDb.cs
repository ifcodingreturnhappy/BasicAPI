using DataLayer.Abstractions;
using System.Linq;

namespace DataLayer.LiteDB
{
    public class LiteDb : IDB
    {
        private const string DB_FILE_EXTENSION = ".db";
        protected readonly string _connectionString;

        public LiteDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected void ValidateconnectionString()
        {
            if (!System.IO.Directory.GetParent(_connectionString).Exists ||
                !_connectionString.Contains("\\"))
            {
                throw new System.Exception("Path provided is invalid - does not exist.");
            }
            else
            {
                if(new System.IO.FileInfo(_connectionString).Extension != DB_FILE_EXTENSION)
                    throw new System.Exception("Path provided is invalid - invalid DB file.");
            }
        }

        protected string GetCollectionName<T>() => typeof(T).Name;
    }
}
