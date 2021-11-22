using System.Collections.Generic;

namespace DataLayer.Abstractions
{
    /// <summary>
    /// This interface represents the READ operations to be executed in a given database.
    /// </summary>
    public interface IDBReader : IDB
    {
        int GetCount<T>();

        /// <summary>
        /// Gets a list of all the data of type T present in the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ReadAll<T>();
        T ReadById<T>(string id);
    }
}
