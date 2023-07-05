using PointSystem.Helper;
using PointSystem.Models.ViewModel;
using PointSystem.Repositories.Repository;
using System.Data.SqlClient;
using System.Data;
using PointSystem.Models.ApiModel;

namespace PointSystem.Repositories.Services
{
    /// <summary>
    /// TransactionServices
    /// </summary>
    /// <seealso cref="PointSystem.Repositories.Repository.ITransactionRepository" />
    public class TransactionServices : ITransactionRepository
    {
        #region Public Method

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <param name="AccountId">The account identifier.</param>
        /// <param name="CompanyId">The company identifier.</param>
        /// <returns></returns>
        public string GetBalance(long accountId, long companyId)
        {
            try
            {
                string balance = string.Empty;
                List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                listSqlParameter.Add(new SqlParameter("AccountId", accountId));
                listSqlParameter.Add(new SqlParameter("CompanyId", companyId));
                List<DataTable> dt = CommonLogic.GetDataTable("prGetLastBalance", listSqlParameter.ToArray(), CommandType.StoredProcedure);

                if (dt != null && dt.Count > 0 && dt[0].Rows.Count > 0)
                {
                    var dtBalance = Convert.ToString(dt[0].Rows[0]["Balance"]);
                    if (!string.IsNullOrEmpty(dtBalance))
                    {
                        balance = Convert.ToString(dtBalance);
                        return balance;
                    }
                }
                return balance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        public List<PointTransactionsViewModel> GetTransaction(TransactionRequestModel transactionRequestModel, long companyId)
        {
            List<PointTransactionsViewModel> listPointTransactionsViewModel = new List<PointTransactionsViewModel>();

            try
            {
                List<SqlParameter> listSqlParameter = new List<SqlParameter>();
                listSqlParameter.Add(new SqlParameter("AccountId", transactionRequestModel.AccountId));
                listSqlParameter.Add(new SqlParameter("CompanyId", companyId));
                listSqlParameter.Add(new SqlParameter("FromDate", transactionRequestModel.FromDate));
                listSqlParameter.Add(new SqlParameter("ToDate", transactionRequestModel.ToDate));

                List<DataTable> dt = CommonLogic.GetDataTable("prGetTransactions", listSqlParameter.ToArray(), CommandType.StoredProcedure);

                if (dt != null)
                {
                    for (int i = 0; i < dt[0].Rows.Count; i++)
                    {
                        var lsttransaction = new PointTransactionsViewModel();
                        lsttransaction = GetTransactionViewModel(dt[0].Rows[i]);
                        if(lsttransaction != null)
                        {
                            listPointTransactionsViewModel.Add(lsttransaction);
                        }
                    }
                }
                return listPointTransactionsViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates the points.
        /// </summary>
        /// <param name="pointRequestModel">The point request model.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        public string AddRewardTransaction(PointsRequestViewModel pointRequestModel, long? companyId)
        {
            decimal currentBalance = 0;
            try
            {
                string Balance = GetBalance(pointRequestModel.AccountId, companyId.HasValue ? companyId.Value : 0);
                if (!string.IsNullOrEmpty(Balance))
                {
                    if (Balance == "No Record Found")
                    {
                        currentBalance = 0;
                    }
                    else
                    {
                        currentBalance = Convert.ToInt32(Balance);
                    }
                }
                else
                {
                    return "Something went wrong while checking a balance.";
                }

                if (pointRequestModel.Points > currentBalance && pointRequestModel.PointType == (int)PointStatus.Redeem)
                {
                    return "You are having insufficient balance.";
                }

                if (pointRequestModel.MultiplesValue > 0 && pointRequestModel.PointType == (int)PointStatus.Earn)
                {
                    // Multuply Point to Value and Update current balance 
                    currentBalance += pointRequestModel.Points * pointRequestModel.MultiplesValue;
                }
                else
                {
                    if (pointRequestModel.PointType == (int)PointStatus.Earn  || pointRequestModel.PointType == (int)PointStatus.AdHoc)
                    {
                        currentBalance += pointRequestModel.Points;// For Earn,For AdHoc
                    }
                    else if (pointRequestModel.PointType == (int)PointStatus.Lose || pointRequestModel.PointType == (int)PointStatus.Redeem) 
                    {
                        currentBalance -= pointRequestModel.Points;// For Lose,For Redeem
                    }
                }

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@AccountId", pointRequestModel.AccountId));
                sqlParameters.Add(new SqlParameter("@CompanyId", companyId));
                sqlParameters.Add(new SqlParameter("@ActionId", pointRequestModel.ActionId));
                sqlParameters.Add(new SqlParameter("@ActionTime", DateTime.UtcNow));
                sqlParameters.Add(new SqlParameter("@PointType", pointRequestModel.PointType));
                sqlParameters.Add(new SqlParameter("@Points", pointRequestModel.Points));
                sqlParameters.Add(new SqlParameter("@Balance", currentBalance));
                sqlParameters.Add(new SqlParameter("@Notes", pointRequestModel.Notes));
                sqlParameters.Add(new SqlParameter("@MultiplesValue", pointRequestModel.MultiplesValue));
                sqlParameters.Add(new SqlParameter("@CreatedOn", DateTime.UtcNow));

                List<DataTable> dts = CommonLogic.GetDataTable("prSavePointDetails", sqlParameters.ToArray(), CommandType.StoredProcedure);
                if (pointRequestModel.PointType == (int)PointStatus.Earn)
                {
                    return "Points earn successfully.";
                }
                else if (pointRequestModel.PointType == (int)PointStatus.Lose)
                {
                    return "Points lose successfully.";
                }
                else if(pointRequestModel.PointType == (int)PointStatus.Redeem)
                {
                    return "Points redeem successfully.";
                }
                else
                {
                    return "Points adhoc successfully.";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Gets the transaction view model.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        private PointTransactionsViewModel GetTransactionViewModel(DataRow dataRow)
        {
            var pointTransactionViewModel = new PointTransactionsViewModel();
            try
            {
                pointTransactionViewModel.PointId = Convert.ToInt64(dataRow["PointId"]);
                pointTransactionViewModel.CompanyId = Convert.ToInt64(dataRow["CompanyId"]);
                pointTransactionViewModel.AccountId = Convert.ToInt64(dataRow["AccountId"]);
                pointTransactionViewModel.ActionId = Convert.ToInt32(dataRow["ActionId"]);
                pointTransactionViewModel.ActionTime = Convert.ToDateTime(dataRow["ActionTime"]);
                pointTransactionViewModel.PointType = Convert.ToInt32(dataRow["PointType"]);
                pointTransactionViewModel.Points = Convert.ToInt32(dataRow["Points"]);
                pointTransactionViewModel.Balance = Convert.ToInt32(dataRow["Balance"]);
                pointTransactionViewModel.Notes = Convert.ToString(dataRow["Notes"]);
                pointTransactionViewModel.MultiplesValue = Convert.ToInt32(dataRow["MultiplesValue"]);
                pointTransactionViewModel.CreatedOn = Convert.ToDateTime(dataRow["CreatedOn"]);
            }
            catch
            {
                return null;
            }
            return pointTransactionViewModel;
        }

        #endregion
    }
}
