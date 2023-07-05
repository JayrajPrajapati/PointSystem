using PointSystem.Helper;
using PointSystem.Models.ViewModel;
using PointSystem.Repositories.Repository;
using System.Data;
using System.Data.SqlClient;

namespace PointSystem.Repositories.Services
{
    /// <summary>
    /// LoginServices
    /// </summary>
    /// <seealso cref="PointSystem.Repositories.Repository.ILoginRepository" />
    public class LoginServices : ILoginRepository
    {
        #region Public Method
        /// <summary>
        /// Checks the login details.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        public string CheckLoginDetails(string apiKey, out long companyId)
        {
            try
            {
                companyId = 0;
                List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                listSqlParameter.Add(new SqlParameter("APIKey", apiKey));
                List<DataTable> dt = CommonLogic.GetDataTable("prCheckLoginDetails", listSqlParameter.ToArray(), CommandType.StoredProcedure);

                if (dt != null && dt.Count > 0 && dt[0].Rows.Count > 0)
                {
                    var drCompany = dt[0].Rows[0];

                    if (drCompany != null)
                    {
                        var loginModel = GetLoginModel(drCompany);
                        if (loginModel != null)
                        {
                            companyId = loginModel.CompanyId;
                            return string.Empty;
                        }
                        else
                        {
                            return "Something went wrong while parsing company details.";
                        }
                    }
                }
                else
                {
                    return "No company found for the provided API key.";
                }

                return "No data found for provided details.";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Gets the login model.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        private LoginViewModel GetLoginModel(DataRow dataRow)
        {
            var loginModel = new LoginViewModel();
            try
            {
                loginModel.CompanyId = Convert.ToInt64(dataRow["CompanyId"]);
                loginModel.CompanyName = Convert.ToString(dataRow["CompanyName"]);
                loginModel.APIKey = Convert.ToString(dataRow["APIKey"]);
                loginModel.CreatedOn = Convert.ToDateTime(dataRow["CreatedOn"]);
            }
            catch
            {
                return null;
            }
            return loginModel;
        }

        #endregion
    }
}
