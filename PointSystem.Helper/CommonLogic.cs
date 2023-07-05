using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PointSystem.Helper
{
    /// <summary>
    /// CommonLogic
    /// </summary>
    public static class CommonLogic
    {
        #region JWT Token

        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="issuer">The issuer.</param>
        /// <returns></returns>
        public static string GenerateJwtToken(string apiKey, string secret, string issuer, long? companyId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("APIKey", apiKey),
                new Claim("CompanyId", Convert.ToString(companyId)),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(Guid.NewGuid()))
            };

            var token = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddDays(1),
                //expires: DateTime.Now.AddMinutes(1),                
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

        #region Database methods

        /// <summary>
        /// Gets the data table.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns></returns>
        public static List<DataTable> GetDataTable(string spName, SqlParameter[] sqlParameters, CommandType commandType)
        {
            DataSet dt = new DataSet();
            List<DataTable> dataTables = new List<DataTable>();
            SqlHelper.GetConnection();
            SqlConnection sqlConnection = new SqlConnection(SqlHelper.ConnectionString);
            sqlConnection.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(spName, sqlConnection);

            try
            {
                sqlDataAdapter.SelectCommand.CommandTimeout = 180;
                sqlDataAdapter.SelectCommand.CommandType = commandType;

                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    sqlDataAdapter.SelectCommand.Parameters.AddRange(sqlParameters);
                }

                sqlDataAdapter.Fill(dt);

                if (dt != null && dt.Tables.Count > 0)
                {
                    dataTables.Add(dt.Tables[0]);
                    if (dt.Tables.Count > 1)
                    {
                        dataTables.Add(dt.Tables[1]);
                    }
                    return dataTables;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlDataAdapter.Dispose();
                dt.Dispose();
                sqlConnection.Close();
            }
        }

        #endregion

    }
}
