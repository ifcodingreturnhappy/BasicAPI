using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Abstractions
{
    /// <summary>
    /// This interface represents the CREATE/UPDATE operations to be executed in a given database.
    /// </summary>
    public interface IDBWriter : IDB
    {
        /// <summary>
        /// Adds a set of type T to the database. If update is set to true, overrides existing ids.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public int Write<T>(IEnumerable<T> data, bool update = true);
        Task<int> WriteAsync<T>(IEnumerable<T> data, bool update = true);
    }
}
