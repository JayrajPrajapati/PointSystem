namespace PointSystem.Models.ViewModel
{
    /// <summary>
    /// PointsRequestModel
    /// </summary>
    public class PointsRequestViewModel
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public long AccountId { get; set; }
        /// <summary>
        /// Gets or sets the action identifier.
        /// </summary>
        /// <value>
        /// The action identifier.
        /// </value>
        public int ActionId { get; set; }
        /// <summary>
        /// Gets or sets the type of the point.
        /// </summary>
        /// <value>
        /// The type of the point.
        /// </value>
        public int PointType { get; set; }
        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public int Points { get; set; }
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }        
        /// <summary>
        /// Gets or sets the multiples value.
        /// </summary>
        /// <value>
        /// The multiples value.
        /// </value>
        public int MultiplesValue { get; set; }
    }
}