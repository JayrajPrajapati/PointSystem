using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointSystem.Helper;
using PointSystem.Model;
using PointSystem.Models.ApiModel;
using PointSystem.Models.ViewModel;
using PointSystem.Repositories.Repository;
using PointSystem.Repositories.Services;
using System.Net.Mime;
using System.Security.Claims;

namespace PointSystem.Controllers
{
    /// <summary>
    /// TransactionController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        #region Variables declaration

        /// <summary>
        /// The transaction repository
        /// </summary>
        private ITransactionRepository transactionRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController" /> class.
        /// </summary>
        /// <param name="_transactionRepository">The transaction repository.</param>
        public TransactionController(ITransactionRepository _transactionRepository)
        {
            transactionRepository = _transactionRepository;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <param name="balanceTransactionModel">The balance transaction model.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("balance")]
        [Consumes(MediaTypeNames.Application.Json)] // "application/json"
        public IActionResult GetBalance([FromQuery] BalanceTransactionModel balanceTransactionModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (balanceTransactionModel != null)
                {
                    if (balanceTransactionModel.AccountId <= 0)
                    {
                        responseModel.Message = "Please provide a valid account id.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    var company = GetUserDetails();
                    if (company == null || (company != null && company.CompanyId <= 0))
                    {
                        responseModel.Message = "The authentication details not found. Please login to access the APIs.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    string balance = transactionRepository.GetBalance(balanceTransactionModel.AccountId, company.CompanyId);

                    if (!string.IsNullOrEmpty(balance) && balance != "No Record Found")
                    {
                        responseModel.Data = Convert.ToInt32(balance);
                        responseModel.Status = (int)APIStatusCodes.SuccessCode;
                    }
                    else
                    {
                        responseModel.Message = "Something went wrong while fetching a balance.";
                    }
                }
                else
                {
                    responseModel.Message = Constants.JsonObjectIsNull;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, responseModel);
        }

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <param name="balanceTransactionModel">The balance transaction model.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("transactionslist")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)] // "application/json"
        public IActionResult GetTransactions([FromQuery] TransactionRequestModel transactionRequestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (transactionRequestModel != null)
                {
                    if (transactionRequestModel.AccountId <= 0)
                    {
                        responseModel.Message = "Please provide a valid account id.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    var company = GetUserDetails();
                    if (company == null || (company != null && company.CompanyId <= 0))
                    {
                        responseModel.Message = "The authentication details not found. Please login to access the APIs.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    List<PointTransactionsViewModel> pointTransactionsViewModel = transactionRepository.GetTransaction(transactionRequestModel, company.CompanyId);

                    if (pointTransactionsViewModel != null && pointTransactionsViewModel.Count > 0)
                    {
                        responseModel.Data = pointTransactionsViewModel;
                        responseModel.Status = (int)APIStatusCodes.SuccessCode;
                    }
                    else
                    {
                        responseModel.Message = "Something went wrong while fetching transactions.";
                    }                    
                }
                else
                {
                    responseModel.Message = Constants.JsonObjectIsNull;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, responseModel);
        }

        /// <summary>
        /// Adds the reward transaction.
        /// </summary>
        /// <param name="pointsRequestModel">The points request model.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("addrewardtransaction")]
        [Consumes(MediaTypeNames.Application.Json)] // "application/json"     
        public IActionResult AddRewardTransaction([FromBody] PointsRequestViewModel pointsRequestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string message = string.Empty;
                if (pointsRequestModel != null)
                {
                    if (pointsRequestModel.AccountId <= 0)
                    {
                        responseModel.Message = "Please provide a valid account id.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    if (pointsRequestModel.ActionId <= 0)
                    {
                        responseModel.Message = "Please provide a valid action id.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    if (pointsRequestModel.PointType != (int)PointStatus.Earn &&
                        pointsRequestModel.PointType != (int)PointStatus.Lose &&
                        pointsRequestModel.PointType != (int)PointStatus.Redeem &&
                        pointsRequestModel.PointType != (int)PointStatus.AdHoc)
                    {
                        responseModel.Message = "Please provide a valid reward type.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    if (pointsRequestModel.Points <= 0)
                    {
                        responseModel.Message = "Please provide a valid reward point.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    if (pointsRequestModel.PointType == (int)PointStatus.Earn && pointsRequestModel.MultiplesValue < 0)
                    {
                        responseModel.Message = "Multiples value must be greater than 0.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    if (pointsRequestModel.PointType != (int)PointStatus.Earn
                        && (pointsRequestModel.MultiplesValue > 0 || pointsRequestModel.MultiplesValue < 0))
                    {
                        responseModel.Message = "Multiples value apply only while earning time.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    var details = GetUserDetails();
                    if (details == null || (details != null && details.CompanyId <= 0))
                    {
                        responseModel.Message = "The authentication details not found. Please login to access the APIs.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    message = transactionRepository.AddRewardTransaction(pointsRequestModel, details.CompanyId);
                    if (!string.IsNullOrEmpty(message))
                    {
                        responseModel.Message = message;
                        responseModel.Status = (int)APIStatusCodes.SuccessCode;
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }
                    else
                    {
                        responseModel.Message = "Something went wrong while adding a reward.";
                    }
                }
                else
                {
                    responseModel.Message = Constants.JsonObjectIsNull;                    
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, responseModel);
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <returns></returns>
        private CompanySession GetUserDetails()
        {
            CompanySession userDetails = null;
            try
            {
                var claims = HttpContext.User.Identity as ClaimsIdentity;
                if (claims != null)
                {
                    var companyId = claims?.FindFirst("CompanyId")?.Value;
                    userDetails = new CompanySession
                    {
                        CompanyId = Convert.ToInt64(companyId),
                    };
                }
            }
            catch
            {
                return null;
            }

            return userDetails;
        }

        #endregion
    }
}
