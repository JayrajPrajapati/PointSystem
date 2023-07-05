namespace PointSystem.Repositories.Repository
{
    /// <summary>
    ///ILoginRepository
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Checks the login details.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        string CheckLoginDetails(string apiKey, out long companyId);
    }
}
