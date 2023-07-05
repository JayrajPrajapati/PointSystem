using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointSystem.Models.ViewModel
{
    /// <summary>
    /// PointTransactionsViewModel
    /// </summary>
    public class PointTransactionsViewModel
    {
        /// <summary>
        /// Gets or sets the point identifier.
        /// </summary>
        /// <value>
        /// The point identifier.
        /// </value>
        public long PointId { get; set; }
        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public long CompanyId { get; set; }
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
        /// Gets or sets the action time.
        /// </summary>
        /// <value>
        /// The action time.
        /// </value>
        public DateTime ActionTime { get; set; }
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
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public int Balance { get; set; }
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string? Notes { get; set; }
        /// <summary>
        /// Gets or sets the multiples value.
        /// </summary>
        /// <value>
        /// The multiples value.
        /// </value>
        public int MultiplesValue { get; set; }
        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public DateTime CreatedOn { get; set; }
    }
}
