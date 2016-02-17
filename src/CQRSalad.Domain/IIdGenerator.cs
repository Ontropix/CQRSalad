namespace CQRSalad.Domain
{
    /// <summary>
    /// Id generator
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates an Id
        /// </summary>
        string Generate();
    }
}