namespace DataLayer.Abstractions
{
    /// <summary>
    /// Basic functionality that every DB communicator should inherit.
    /// </summary>
    public interface IDB
    {
        /// <summary>
        /// Checks if the connection string passed in is valid. 
        /// </summary>
        public void ValidateconnectionString()
        {
        }
    }
}
