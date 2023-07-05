using PointSystem.Helper;

namespace PointSystem.Models.ApiModel
{
    /// <summary>
    /// ResponseModel
    /// </summary>
    public class ResponseModel
    {
        #region Properties

        /// <summary>Gets or sets a value indicating whether this <see cref="ResponseModel"/> is success.</summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.</value>
        public int Status { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>Gets or sets the data.</summary>
        /// <value>The data.</value>
        public object Data { get; set; }

        #endregion

        #region Constructor

        /// <summary>Initializes a new instance of the <see cref="ResponseModel"/> class.</summary>
        public ResponseModel()
        {
            Status = (int)APIStatusCodes.FailureCode;
            Message = string.Empty;
            Data = new object();
        }

        #endregion
    }
}
