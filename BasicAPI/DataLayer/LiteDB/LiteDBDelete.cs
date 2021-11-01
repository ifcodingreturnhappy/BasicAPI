using DataLayer.Abstractions;
using DataLayer.LiteDB;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataLayer.LiteDB
{
    public class LiteDBDelete : LiteDb, IDBDeleter
    {
        public LiteDBDelete(string connectionString) : base(connectionString)
        {
            ValidateconnectionString();
        }

        public int Delete(DbEntityBase data)
        {
            var result = 0;

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var colName = GetCollectionName<DbEntityBase>();
                    var col = db.GetCollection<DbEntityBase>(colName);

                    var deleteQuery = $"DELETE {colName} WHERE _id == {data.Id}";
                    col.DeleteMany(deleteQuery);
                    //col.Delete(data.Id);
                }
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }

        public int DeleteAll<T>()
        {
            var result = 0;

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(GetCollectionName<T>());

                    result = col.DeleteAll();
                }
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }
    }
}
