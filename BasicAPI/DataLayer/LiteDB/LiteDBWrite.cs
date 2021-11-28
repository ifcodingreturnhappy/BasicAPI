using DataLayer.Abstractions;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.LiteDB
{
    public class LiteDBWrite : LiteDb, IDBWriter
    {
        public LiteDBWrite(string connectionString) : base(connectionString)
        {
            ValidateconnectionString();
        }


        public int WriteMany<T>(IEnumerable<T> data, bool update = true)
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
                    {
                        result = col.Upsert(data);
                        if (result == 0)
                            result = col.Update(data);
                    }
                    else
                    {
                        result = col.Insert(data);
                    }
                }
            }
            catch (Exception e)
            {
                // Log
                result = -1;
            }

            return result;
        }

        public async Task<int> WriteManyAsync<T>(IEnumerable<T> data, bool update = true)
        {
            var result = await Task.Run(() =>
            {
                return WriteMany(data, update);
            });

            return result;
        }


        public int Write<T>(T data, bool update = true)
        {
            var result = 0;

            try
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<T>(GetCollectionName<T>());

                    // Add objects to the collection
                    if(update)
                    {
                        // Try update
                        var updateSuccess = col.Update(data);

                        // If update unsucessful (means no ID was found in collection, try insert)
                        if (!updateSuccess)
                        {
                            var insertResult = col.Insert(data);
                            result = insertResult != null ? 1 : 0;
                        }
                        else
                        {
                            result = updateSuccess ? 1 : 0;
                        }
                    }
                    else
                    {
                        var insertSuccess = col.Insert(data);
                        result = insertSuccess != null ? 1 : 0;
                    }
                }
            }
            catch (Exception e)
            {
                // Log
                result = -1;
            }

            return result;
        }

        public async Task<int> WriteAsync<T>(T data, bool update = true)
        {
            var result = await Task.Run(() =>
            {
                return Write(data, update);
            });

            return result;
        }
    }
}
