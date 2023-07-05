using PointSystem.Helper;
using PointSystem.Models.ApiModel;
using PointSystem.Models.ViewModel;
using PointSystem.Repositories.Repository;
using System.Data;
using System.Data.SqlClient;

namespace PointSystem.Repositories.Services
{
    /// <summary>
    /// CompanyServices
    /// </summary>
    /// <seealso cref="PointSystem.Repositories.Repository.ICompanyRepository" />
    public class CompanyServices : ICompanyRepository
    {
        #region Public Method

        /// <summary>
        /// Checks the auth Key.
        /// </summary>
        /// <param name="authKey">The auth key.</param>
        /// <returns></returns>
        public string CheckAuthKey(string authKey)
        {
            try
            {
                List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                listSqlParameter.Add(new SqlParameter("AuthKey", authKey));
                List<DataTable> dt = CommonLogic.GetDataTable("prCheckAuthenticationWithAuthKEY", listSqlParameter.ToArray(), CommandType.StoredProcedure);

                if (dt != null && dt.Count > 0 && dt[0].Rows.Count > 0)
                {
                    var drCompany = dt[0].Rows[0];

                    if (drCompany != null)
                    {
                        var settingModel = GetSettingViewModel(drCompany);
                        if (settingModel != null)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return "Something went wrong while parsing authentication key details.";
                        }
                    }
                }
                else
                {
                    return "No company found for the provided API key.";
                }

                return "No data found with provided details.";

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the company.
        /// </summary>
        /// <param name="companyRequestModel">The company request model.</param>
        /// <returns></returns>
        public string CreateCompany(CompanyRequestModel companyRequestModel)
        {
            string guid = string.Empty;
            try
            {
                guid = Guid.NewGuid().ToString();
                List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                listSqlParameter.Add(new SqlParameter("@companyName", companyRequestModel.CompanyName));
                listSqlParameter.Add(new SqlParameter("@apiKey", guid));
                listSqlParameter.Add(new SqlParameter("@createdOn", DateTime.UtcNow));
                List<DataTable> dt = CommonLogic.GetDataTable("prSaveCompany", listSqlParameter.ToArray(), CommandType.StoredProcedure);

                if (dt != null && dt.Count > 0 && dt[0].Rows.Count > 0)
                {
                    string drCompany = Convert.ToString(dt[0].Rows[0]["Company"]);
                    if (!string.IsNullOrEmpty(drCompany))
                    {
                        return guid = drCompany;
                    }
                }

                return guid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Gets the setting view model.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        private SettingViewModel GetSettingViewModel(DataRow dataRow)
        {
            var settingModel = new SettingViewModel();
            try
            {
                settingModel.SettingId = Convert.ToInt64(dataRow["SettingId"]);
                settingModel.AuthKey = Convert.ToString(dataRow["AuthKey"]);
            }
            catch
            {
                return null;
            }

            return settingModel;
        }

        #endregion
    }
}