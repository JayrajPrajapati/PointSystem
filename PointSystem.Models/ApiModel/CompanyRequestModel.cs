namespace PointSystem.Models.ApiModel
{
    /// <summary>
    /// CompanyRequestModel
    /// </summary>
    public class CompanyRequestModel
    {
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string AuthKey { get; set; }
    }
}