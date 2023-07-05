using PointSystem.Models.ApiModel;
using PointSystem.Models.ViewModel;

namespace PointSystem.Repositories.Repository
{
    /// <summary>
    /// ITransactionRepository
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        string GetBalance(long accountId, long companyId);

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        List<PointTransactionsViewModel> GetTransaction(TransactionRequestModel transactionRequestModel, long companyId);

        /// <summary>
        /// Creates the points.
        /// </summary>
        /// <param name="pointRequestModel">The point request model.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        string AddRewardTransaction(PointsRequestViewModel pointRequestModel, long? companyId);
    }
}