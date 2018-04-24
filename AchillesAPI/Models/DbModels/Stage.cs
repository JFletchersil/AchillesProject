using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchillesAPI.Models.DbModels
{
    /// <summary>
    /// The stage model.
    /// </summary>
    [Table("Stages")]
    public class Stage
    {
        /// <summary>
        /// Gets or sets the stage identifier.
        /// </summary>
        /// <value>
        /// The stage identifier.
        /// </value>
        [Key]
        public Guid StageID { get; set; }
        /// <summary>
        /// Gets or sets the stage number.
        /// </summary>
        /// <value>
        /// The stage number.
        /// </value>
        public int StageNumber { get; set; }
    }
}
