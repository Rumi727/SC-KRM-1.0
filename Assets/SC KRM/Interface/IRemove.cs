namespace SCKRM
{
    public interface IRemove
    {
        public bool isRemoved { get; }

        /// <summary>
        /// Object Remove
        /// </summary>
        /// <returns>Is Remove Success</returns>
        bool Remove();
    }

    public interface IRemoveForce : IRemove
    {
        /// <summary>
        /// Object Remove
        /// </summary>
        /// <returns>Is Remove Success</returns>
        bool Remove(bool force);
    }
}
