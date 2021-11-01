using DataLayer.Abstractions;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.LiteDB
{
    public class LiteDBWrite : LiteDb, IDBWriter
    {
        public LiteDBWrite(string connectionString) : base(connectionString)
        {
            ValidateconnectionString();
        }


        public int Write<T>(IEnumerable<T> data, bool update)
        {
            var result = 0;

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(GetCollectionName<T>());

                    // Add objects to the collection
                    if (update)
                        result = col.Upsert(data);
                    else
                        result = col.Insert(data);
                }
            }
            catch (Exception)
            {
                // Log
                result = -1;
            }

            return result;
        }
    }
}
