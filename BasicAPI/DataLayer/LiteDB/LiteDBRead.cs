using DataLayer.Abstractions;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.LiteDB
{
    public class LiteDBRead : LiteDb, IDBReader
    {
        public LiteDBRead(string connectionString) : base(connectionString)
        {
            ValidateconnectionString();
        }

        public IEnumerable<T> ReadAll<T>()
        {
            List<T> result = new List<T>();

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(GetCollectionName<T>());

                    result = col.FindAll().ToList();
                }
            }
            catch (Exception)
            {
                // Log
            }

            return result;
        }

        public T ReadById<T>(string id)
        {
            T result = default(T);

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(GetCollectionName<T>());

                    result = col.FindById(id);
                }
            }
            catch (Exception)
            {
                // Log
            }

            return result;
        }

        public int GetCount<T>()
        {
            int result = 0;

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(GetCollectionName<T>());

                    result = col.Count();
                }
            }
            catch (Exception)
            {
                // Log
            }

            return result;
        }
    }
}
