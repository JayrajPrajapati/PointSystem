using PointSystem.Models.ApiModel;

namespace PointSystem.Repositories.Repository
{
    /// <summary>
    /// ICompanyRepository
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// Checks the Auth key.
        /// </summary>
        /// <param name="authKey">The Auth key.</param>
        /// <returns></returns>
        string CheckAuthKey(string authKey);

        /// <summary>
        /// Creates the company.
        /// </summary>
        /// <param name="companyRequestModel">The company request model.</param>
        /// <returns></returns>
        string CreateCompany(CompanyRequestModel companyRequestModel);
    }
}