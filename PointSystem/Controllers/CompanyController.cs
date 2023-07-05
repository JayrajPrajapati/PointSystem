using Microsoft.AspNetCore.Mvc;
using PointSystem.Helper;
using PointSystem.Models.ApiModel;
using PointSystem.Repositories.Repository;
using System.Net.Mime;

namespace PointSystem.Controllers
{
    /// <summary>
    /// CompanyController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {

        #region Variables declaration

        /// <summary>
        /// The company repository
        /// </summary>
        private ICompanyRepository companyRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyController" /> class.
        /// </summary>
        /// <param name="_companyRepository">The login repository.</param>
        public CompanyController(ICompanyRepository _companyRepository)
        {
            companyRepository = _companyRepository;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Creates the company.
        /// </summary>
        /// <param name="companyModel">The company model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [Consumes(MediaTypeNames.Application.Json)] // "application/json"
        public IActionResult CreateCompany([FromBody] CompanyRequestModel companyModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (companyModel != null)
                {
                    if (string.IsNullOrEmpty(companyModel.AuthKey))
                    {
                        responseModel.Message = "Please provide an authentication key.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }
                    if (string.IsNullOrEmpty(companyModel.CompanyName))
                    {
                        responseModel.Message = "Please provide a company name.";
                        return StatusCode(StatusCodes.Status200OK, responseModel);
                    }

                    string message = companyRepository.CheckAuthKey(companyModel.AuthKey);
                    if (string.IsNullOrEmpty(message))
                    {
                        string apikey = companyRepository.CreateCompany(companyModel);
                        if (!string.IsNullOrEmpty(apikey))
                        {
                            if (apikey == "100")
                            {
                                responseModel.Message = "Company name already exist.";
                            }
                            else
                            {
                                responseModel.Message = "Company created successfully.";
                                responseModel.Status = (int)APIStatusCodes.SuccessCode;
                                responseModel.Data = apikey;
                            }
                        }
                        else
                        {
                            responseModel.Message = "Something went wrong while creating a company.";
                        }
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