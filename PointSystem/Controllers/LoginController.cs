using Microsoft.AspNetCore.Mvc;
using PointSystem.Helper;
using PointSystem.Models.ApiModel;
using PointSystem.Repositories.Repository;
using System.Net.Mime;

namespace PointSystem.Controllers
{
    /// <summary>
    /// LoginController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        #region Variables declaration

        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration configuration;

        /// <summary>
        /// The login repository
        /// </summary>
        private ILoginRepository loginRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="_configuration">The configuration.</param>
        /// <param name="_loginRepository">The login repository.</param>
        public LoginController(IConfiguration _configuration, ILoginRepository _loginRepository)
        {
            configuration = _configuration;
            loginRepository = _loginRepository;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Logins the specified login model.
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [Consumes(MediaTypeNames.Application.Json)] // "application/json"        
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (loginModel != null)
                {
                    if (string.IsNullOrEmpty(loginModel.APIKey))
                    {
                        responseModel.Message = "Please provide an API Key.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }
                    
                    string message = loginRepository.CheckLoginDetails(loginModel.APIKey, out long companyId);

                    if (string.IsNullOrEmpty(message))
                    {
                        string token = CommonLogic.GenerateJwtToken(loginModel.APIKey, Convert.ToString(configuration["app_settings:Secret"]), Convert.ToString(configuration["app_settings:Issuer"]), companyId);

                        responseModel.Message = "Logged in successfully";
                        responseModel.Data = token;
                        responseModel.Status = (int)APIStatusCodes.SuccessCode;
                    }
                    else
                    {
                        responseModel.Message = message;
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
    }
}
