using System.Collections.Generic;

namespace DataLayer.Abstractions
{
    /// <summary>
    /// This interface represents the DELETE operations to be executed in a given database.
    /// </summary>
    public interface IDBDeleter : IDB
    {
        /// <summary>
        /// Deletes all data of type T in the given database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int DeleteAll<T>();

        /// <summary>
        /// Deletes the item for the given id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Delete(DbEntityBase data);
    }
}
